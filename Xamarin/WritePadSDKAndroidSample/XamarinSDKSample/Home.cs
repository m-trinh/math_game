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
    public class Home : Activity
    {

		private TextView usernameTextBox;
		private TextView levelTextBox;
		private Button madMinuteGame;
		private Button statistics;
		private Button leaderboards;
		private string username = User.username;

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
            usernameTextBox = FindViewById<TextView>(Resource.Id.username);
			//Receiving the information of the user from LoginActivity.
			usernameTextBox.Text = User.firstName;

			//Show user level
			levelTextBox = FindViewById<TextView> (Resource.Id.lvl);
			levelTextBox.Text = $"LVL: {User.level}";

            /*
			FACEBOOK CONNECTION
			*/
            FacebookSdk.SdkInitialize(ApplicationContext);

            LoginButton buttonFacebook = FindViewById<LoginButton>(Resource.Id.login_button);

            //Whenever we click on logout facebook button, it should take us to the login page
            buttonFacebook.Click += delegate {
                LoginManager.Instance.LogOut();
                StartActivity(typeof(LoginActivity));
            };

            /*
			MAD MINUTE
			*/
            madMinuteGame = FindViewById<Button>(Resource.Id.madminutegame);
            madMinuteGame.Click += delegate {
				var madminute = new Intent (this, typeof (MadMinute));
				madminute.PutExtra ("mode", "normal");
                StartActivity(madminute);
            };

			/*
			 TRAINING GAME MODE
			 */
			var trainingbutton = FindViewById<Button> (Resource.Id.traininggame);
            trainingbutton.Click += delegate {
				var madminute = new Intent (this, typeof (MadMinute));
				madminute.PutExtra ("mode", "training");			    StartActivity(madminute);
            };

			/*STATISTICS
			*/
			statistics = FindViewById<Button>(Resource.Id.profile);
			statistics.Click += delegate
			{
                StartActivity(typeof (GenerateStats));
			};

			/*
			 SAT GAME MODE
			 */
			var satbutton = FindViewById<Button> (Resource.Id.satgame);
            satbutton.Click += delegate {
                StartActivity(typeof(SATLevelChoose));
            };

			/*
			 LEADERBOARDS			 
			 */
			leaderboards = FindViewById<Button> (Resource.Id.leaderboards);
			leaderboards.Click += delegate {
				StartActivity (typeof (LeaderboardActivity));
			};
        }
    }
}