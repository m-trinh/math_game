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
            var button = FindViewById<Button> (Resource.Id.traininggame);

			button.Click += delegate {
				StartActivity (typeof (Activity1));
			};
        }
    }
}