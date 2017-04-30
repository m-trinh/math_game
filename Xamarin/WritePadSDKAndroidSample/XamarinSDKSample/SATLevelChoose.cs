using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Data.SqlClient;

namespace WritePadXamarinSample
{
	[Activity (Label = "Activity3")]
	public class SATLevelChoose : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.LevelChoose);

			string username = User.username;

			int [] starImages = { Resource.Drawable.stars0, Resource.Drawable.stars1, Resource.Drawable.stars2, Resource.Drawable.stars3, Resource.Drawable.stars4, Resource.Drawable.stars5 };
			int [] starsByLevel = { 0, 0, 0, 0 };

			var level1Stars = FindViewById<ImageView> (Resource.Id.level1Stars);
			level1Stars.SetImageResource (Resource.Drawable.stars0);

			var level2Stars = FindViewById<ImageView> (Resource.Id.level2Stars);
			level2Stars.SetImageResource (Resource.Drawable.stars0);

			var level3Stars = FindViewById<ImageView> (Resource.Id.level3Stars);
			level3Stars.SetImageResource (Resource.Drawable.stars0);

			var level4Stars = FindViewById<ImageView> (Resource.Id.level4Stars);
			level4Stars.SetImageResource (Resource.Drawable.stars0);

			var level1 = FindViewById<LinearLayout> (Resource.Id.level1);
			var level2 = FindViewById<LinearLayout> (Resource.Id.level2);
			var level3 = FindViewById<LinearLayout> (Resource.Id.level3);
			var level4 = FindViewById<LinearLayout> (Resource.Id.level4);

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
							starsByLevel [0] = (int)reader ["NUMBER_OF_STARS"];
                        }
                        else if ((string)reader["CATEGORY"] == "Geometry" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level2Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
							starsByLevel [1] = (int)reader ["NUMBER_OF_STARS"];
                        }
                        else if ((string)reader["CATEGORY"] == "Trigonometry" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level3Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
							starsByLevel [2] = (int)reader ["NUMBER_OF_STARS"];
                        }
                        else if ((string)reader["CATEGORY"] == "Challenge" && (string)reader["DIFFICULTY"] == "Easy")
                        {
                            level4Stars.SetImageResource(starImages[(int)reader["NUMBER_OF_STARS"]]);
							starsByLevel [3] = (int)reader ["NUMBER_OF_STARS"];
                        }
                    }
                }
                connection.Close();
            }

			if (starsByLevel [0] == 5) {
                chooseMode (level1, "Algebra", "Easy");
			} else {
                designateButton (level1, "Algebra", "Easy", 0);
			}

			if (starsByLevel [1] == 5) {
                chooseMode (level2, "Geometry", "Easy");
			} else {
				designateButton (level2, "Geometry", "Easy", 0);
			}

			if (starsByLevel [2] == 5) {
                chooseMode (level3, "Trigonometry", "Easy");
			} else {
				designateButton (level3, "Trigonometry", "Easy", 0);
			}

			if (starsByLevel [3] == 5) {
                chooseMode (level4, "Challenge", "Easy");
			} else {
				designateButton (level4, "Challenge", "Easy", 0);
			}
        }

		public void designateButton (LinearLayout level, string category, string difficulty, int mode)
		{
			var QuestionActivity = getQuestionActivity (category, difficulty, mode);
			level.Click += delegate 
			{
				StartActivity (QuestionActivity);
			};
		}

		public void chooseMode (LinearLayout level, string category, string difficulty)
		{
			int normalMode = 0;
			int reviewMode = 1;

			level.Click += delegate {
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("Choose a Mode");
				builder.SetPositiveButton ("Normal", delegate { StartActivity(getQuestionActivity(category, difficulty, normalMode)); });
				builder.SetNeutralButton ("Review", delegate { StartActivity(getQuestionActivity(category, difficulty, reviewMode)); });
				builder.Show ();
			};
		}

		public Intent getQuestionActivity (string category, string difficulty, int mode)
		{
			var QuestionActivity = new Intent (this, typeof (QuestionActivity));
			if (category == "Challenge") {
				QuestionActivity = new Intent (this, typeof (ChallengeQuestionActivity));
			}

			QuestionActivity.PutExtra ("category", category).PutExtra ("difficulty", difficulty).PutExtra ("mode", mode);

			return QuestionActivity;
		}
	}
}
