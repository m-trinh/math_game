
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
	[Activity (Label = "Activity3")]
	public class Activity3 : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.LevelChoose);

			var image = FindViewById<ImageView> (Resource.Id.imageView1);
			image.SetImageResource (Resource.Drawable.stars0);
            // Create your application here

            var button = FindViewById<LinearLayout>(Resource.Id.level1);

            button.Click += delegate
            {
                StartActivity(typeof(QuestionActivity));
            };
		}
	}
}
