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
    [Activity(Label = "Leaderboards")]
    public class LeaderboardActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Leaderboards);

            var leaderboardArea = FindViewById<LinearLayout>(Resource.Id.leaderboards);

            int timeframe = 0;
			int score;
			string user;

            SqlConnectionStringBuilder builder = ConnString.Builder;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append($"EXEC usp_RetrieveMadMinuteLeaderboard {timeframe}");
                string query = sb.ToString();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int position = 1;
                    while (reader.Read())
                    {
						var newrow = createNewRow();
                        score = (int)reader ["Score"];
						user = (string)reader ["Username"];
						newrow.Text = $"#{position}: {user} - {score} points";
                        leaderboardArea.AddView(newrow);
                        position++;
                    }
                }
                connection.Close();
            }
                
            // Create your application here
        }

		public TextView createNewRow ()
		{
			var newrow = new TextView (this);
			newrow.SetTextSize (Android.Util.ComplexUnitType.Dip, 15f);
			newrow.SetPadding (30, 10, 30, 10);
			newrow.SetBackgroundResource (Resource.Drawable.border);
			newrow.SetTextColor (Android.Graphics.Color.Black);
			return newrow;
		}
    }
}