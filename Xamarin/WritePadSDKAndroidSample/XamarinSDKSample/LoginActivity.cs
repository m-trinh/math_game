using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using Plugin.Geolocator;
using System.Net.Http;

namespace WritePadXamarinSample
{
	/**
	 * The class loginActivity alows a user to login to the app by using the Facebook API
	 */
	[Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon")]
	public class LoginActivity : Activity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
	{
		private ICallbackManager mCallBackManager;
		private MyProfileTracker mProfileTracker;
		private UserInfo userLoggedInInfo;
		UserInfo newUser;

		//Listener 
		/**
		 * mProgileTracker_MOnProfileChange tracks the changes in the user, when the user accepts the facebook
		 * login through the app, then the login is valid and you go to your account in the game.
		 */
		void mProfileTracker_MOnProfileChange(object sender, OnProfileChangeEventArgs e)
		{
			if (e.mProfile != null)
			{
				//Create a new user
				userLoggedInInfo = new UserInfo(e.mProfile.FirstName, e.mProfile.LastName, e.mProfile.Name);

				User.firstName = e.mProfile.FirstName;
				User.lastName = e.mProfile.LastName;
				User.username = e.mProfile.FirstName + e.mProfile.LastName;

				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;

				var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				var pathToDatabase = System.IO.Path.Combine(docsFolder, "MathAttackDatabase.db");

				//Using the previous method we only obtained the public profile of the user
				//The public profile only return the profile picture, first name, last name and id
				//However we wanted also the email of the user.
				//In order to be able to retrieve the user email we would need to use the facebook graphApi.
				//This graph api sends a request to the facebook api and returns a json
				GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, (Xamarin.Facebook.GraphRequest.IGraphJSONObjectCallback)(this));
				Bundle parameters = new Bundle();
				parameters.PutString("fields","id,name,email");//Fields I want to retrieve
				request.Parameters = parameters;
				//When the request is successfull the method called is onCompleted()
				request.ExecuteAsync();
				//newUser.Email
			}
		}

		public void retrieveInfo(GraphRequest request)
		{
			request.ExecuteAsync();
		}

		public void OnCancel()
		{
			//throw new NotImplementedException();
			mProfileTracker.StopTracking();
			base.OnDestroy();
		}

		public void OnError(FacebookException p0)
		{
			//throw new NotImplementedException();
		}

		public void OnSuccess(Java.Lang.Object result)
		{
			LoginResult loginResult = result as LoginResult;
			//UserId is the one we should save in the database of our app
			Console.Write(loginResult.AccessToken.UserId);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			mProfileTracker.StopTracking();
			LoginManager.Instance.LogOut();
			StartActivity(typeof(LoginActivity));
		}
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			LocationFinder myLocation = new LocationFinder();
			myLocation.findPosition();

			FacebookSdk.SdkInitialize(this.ApplicationContext);

			//We start tracking the profile to capture if it changes
			mProfileTracker = new MyProfileTracker();
			mProfileTracker.mOnProfileChange += mProfileTracker_MOnProfileChange;
			mProfileTracker.StartTracking();


			SetContentView(Resource.Layout.LoginPage);

			if (AccessToken.CurrentAccessToken != null)
			{
				StartActivity(typeof(Activity2));
			}


			LoginButton button = FindViewById<LoginButton>(Resource.Id.login_button);

			//Activate what fields we want to retrieve in the facebook developer page
			button.SetReadPermissions(new List<string> { "public_profile", "user_friends", "email" });

			mCallBackManager = CallbackManagerFactory.Create();

			button.RegisterCallback(mCallBackManager, this);


		}

		/**
		 * OnCompleted is executed when the asyncronous task of requesting the user email is completed.
		 * @postcondition:
		 * 	When the JSON is returned we parse it and add the value of the email into the user class, and call
		 *  the class loadNewActivity()
		 */ 
		public void OnCompleted(JSONObject p0, GraphResponse p1)
		{
			var obj = JObject.Parse((string)p0);
 			User.email = (string)obj["email"];
			loadNewActivity();
		}
		/**
		 * loadNewActivity loads the new activity for the user.
		 * @Postcondition:
		 * 	The values of the user are store into the database and the new view will load.
		 */ 
		private void loadNewActivity()
		{
			//In order to pass the information through activities we will have to use JSON. Cast the user to a json document
			//Pass to the new activity the username and the last name of the user
			var activity2 = new Intent(this, typeof(Activity2));

			//Save in the database the values of the user
			ConnectToDatabase saveInfo = new ConnectToDatabase();
			saveInfo.saveUser();

			//Start the new activity which is the home page.
			StartActivity(activity2);
		}
	}


	public class MyProfileTracker : ProfileTracker
	{
		public event EventHandler<OnProfileChangeEventArgs> mOnProfileChange;
		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
		{
			if (mOnProfileChange != null)
			{
				mOnProfileChange.Invoke(this, new OnProfileChangeEventArgs(newProfile));
			}
		}
	}
	public class OnProfileChangeEventArgs : EventArgs
	{
		public Profile mProfile;

		public OnProfileChangeEventArgs(Profile profile)
		{
			mProfile = profile;
		}
	}


}