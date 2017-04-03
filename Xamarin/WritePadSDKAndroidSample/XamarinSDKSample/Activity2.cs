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

namespace WritePadXamarinSample
{
    [Activity(Label = "Activity2", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity2 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Home);
            var trainingbutton = FindViewById<Button> (Resource.Id.traininggame);
			var satbutton = FindViewById<Button> (Resource.Id.satgame);

			trainingbutton.Click += delegate {
				StartActivity (typeof (Activity1));
			};

			satbutton.Click += delegate {
				StartActivity (typeof (Activity3));
			};
        }
    }
}