
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
using System.Data.SqlClient;

namespace WritePadXamarinSample
{
	[Activity (Label = "Activity3")]
	public class Activity3 : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.LevelChoose);

            string username = "MIKE";

            int[] starImages = new int[] { Resource.Drawable.stars0, Resource.Drawable.stars1, Resource.Drawable.stars2, Resource.Drawable.stars3, Resource.Drawable.stars4, Resource.Drawable.stars5 };

            var level1Stars = FindViewById<ImageView>(Resource.Id.level1Stars);
            level1Stars.SetImageResource(Resource.Drawable.stars0);

            var level2Stars = FindViewById<ImageView>(Resource.Id.level2Stars);
            level2Stars.SetImageResource(Resource.Drawable.stars0);

            var level3Stars = FindViewById<ImageView>(Resource.Id.level3Stars);
            level3Stars.SetImageResource(Resource.Drawable.stars0);

            var level4Stars = FindViewById<ImageView>(Resource.Id.level4Stars);
            level4Stars.SetImageResource(Resource.Drawable.stars0);

            SqlConnectionStringBuilder builder = ConnString.Builder;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM dbo.SAT_STARS_BY_USER WHERE USERNAME = '{username}'";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((string) reader["CATEGORY"] == "Algebra" && (string) reader["DIFFICULTY"] == "Easy")
                        {
                            level1Stars.SetImageResource(starImages[(int) reader["NUMBER_OF_STARS"]]);
                        }
                        else if ((string)reader["CATEGORY"] == "Geometry" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level2Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
                        }
                        else if ((string)reader["CATEGORY"] == "Trigonometry" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level3Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
                        }
                        else if ((string)reader["CATEGORY"] == "Challenge" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level4Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
                        }
                    }
                }
                connection.Close();
            }

            var level1 = FindViewById<LinearLayout>(Resource.Id.level1);
            var level2 = FindViewById<LinearLayout>(Resource.Id.level2);
            var level3 = FindViewById<LinearLayout>(Resource.Id.level3);
            var level4 = FindViewById<LinearLayout>(Resource.Id.level4);

            var QuestionActivity = new Intent(this, typeof(QuestionActivity));
			var ChallengeQuestionActivity = new Intent(this, typeof(ChallengeQuestionActivity));

            IList<string> details;
            level1.Click += delegate
            {
                details = new List<string>() { username, "Algebra", "Easy" };
                QuestionActivity.PutStringArrayListExtra("details", details);
                StartActivity(QuestionActivity);
            };
            level2.Click += delegate
            {
                details = new List<string>() { username, "Geometry", "Easy" };
                QuestionActivity.PutStringArrayListExtra("details", details);
                StartActivity(QuestionActivity);
            };
            level3.Click += delegate
            {
                details = new List<string>() { username, "Trigonometry", "Easy" };
                QuestionActivity.PutStringArrayListExtra("details", details);
                StartActivity(QuestionActivity);
            };
            level4.Click += delegate
            {
                details = new List<string>() { username, "Challenge", "Easy" };
                ChallengeQuestionActivity.PutStringArrayListExtra("details", details);
                StartActivity(ChallengeQuestionActivity);
            };
        }
	}
}
