using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace WritePadXamarinSample
{

	[Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon")]
	public class LoginActivity : Activity, IFacebookCallback, GraphRequest.IGraphJSONArrayCallback
	{
		private ICallbackManager mCallBackManager;
		private MyProfileTracker mProfileTracker;
		private UserInfo userLoggedInInfo;

		//Listener 
		void mProfileTracker_MOnProfileChange(object sender, OnProfileChangeEventArgs e)
		{
			if (e.mProfile != null)
			{

				userLoggedInInfo = new UserInfo(e.mProfile.FirstName, e.mProfile.LastName, e.mProfile.Name);

				var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				var pathToDatabase = System.IO.Path.Combine(docsFolder, "MathAttackDatabase.db");

				UserInfo newUser = new UserInfo();
				newUser.FirstName = e.mProfile.FirstName;
				newUser.LastName = e.mProfile.LastName;
				//newUser.Email = e.mProfile.Email;


				//In order to pass the information through activities we will have to use JSON. Cast the user to a json document
				var activity2 = new Intent(this, typeof(Activity2));
				activity2.PutExtra("UserName", e.mProfile.FirstName);
				activity2.PutExtra("UserLastName", e.mProfile.LastName);
				//activity2.PutExtra ("UserEmail", e.mProfile.Email);
				StartActivity(activity2);

			}

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
			button.SetReadPermissions(new List<string> { "public_profile", "email", "user_friends" });

			mCallBackManager = CallbackManagerFactory.Create();

			button.RegisterCallback(mCallBackManager, this);


		}

		public void OnCompleted(JSONArray json, GraphResponse p1)
		{
			//throw new NotImplementedException ();
			Console.Write(json.ToString());
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