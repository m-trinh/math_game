using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace WritePadXamarinSample
{
    [Activity(Label = "Home", MainLauncher = false, Icon = "@drawable/icon")]
    public class Activity2 : Activity
    {

		private TextView usernameTextBox;
		private Button madMinuteGame;
		private Button statistics;
		private string username;

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			LoginManager.Instance.LogOut ();
		}

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Home);

			/*
			RETRIEVE USER'S INFORMATION
			*/
			usernameTextBox = FindViewById<TextView> (Resource.Id.username);
			//Receiving the information of the user from LoginActivity.
			username = Intent.GetStringExtra ("UserName");
			usernameTextBox.Text = username;

			/*
			FACEBOOK CONNECTION
			*/
			FacebookSdk.SdkInitialize (ApplicationContext);

			LoginButton buttonFacebook = FindViewById<LoginButton> (Resource.Id.login_button);

			//Whenever we click on logout facebook button, it should take us to the login page
			buttonFacebook.Click += delegate {
				LoginManager.Instance.LogOut ();
				StartActivity (typeof (LoginActivity));
			};

			/*
			MAD MINUTE
			*/

			madMinuteGame = FindViewById<Button> (Resource.Id.madminutegame);

			madMinuteGame.Click += delegate {
				var madMinute = new Intent (this, typeof (MadMinute));
				madMinute.PutExtra ("UserName", username);
				//activity2.PutExtra ("UserEmail", e.mProfile.Email);
				StartActivity (madMinute);
			};

			/*
			SAT QUESTIONS
			*/
            var trainingbutton = FindViewById<Button> (Resource.Id.traininggame);
			var satbutton = FindViewById<Button> (Resource.Id.satgame);

			trainingbutton.Click += delegate {
				StartActivity (typeof (Activity1));
			};

			satbutton.Click += delegate {
				StartActivity (typeof (Activity3));
			};

			/*
			 * STATISTICS
			*/
			statistics = FindViewById<Button>(Resource.Id.profile);
			statistics.Click += delegate
			{
				var statsUsername = new Intent (this, typeof (GenerateStats));
				statsUsername.PutExtra ("UserName", username);
				StartActivity(statsUsername);
			};
        }
    }
}