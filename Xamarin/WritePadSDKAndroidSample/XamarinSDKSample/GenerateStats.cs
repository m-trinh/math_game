using Android.App;
using Android.OS;
using System;
using Android.Views;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using System.Collections;
using Android.Widget;
using System.Data.SqlClient;
using System.Text;

namespace WritePadXamarinSample
{
	/**
	 * Generate Stats will allow the users to see how they are performing in the mad minute game!
	 * 
	 * The API used to plot the graph is called Oxyplot
	 * Oxyplot allows to show a linear graph.
	 * 
	 */
	[Activity (Label = "Statistics", MainLauncher = false)]
	public class GenerateStats : Activity
	{
		
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Stats);

			//Getting the view that the stats are going to be plotted
			PlotView view = FindViewById<PlotView> (Resource.Id.plot_view);
			string username = User.username;

			//Create the View
			view.Model = CreatePlotModel(username);

			//Add leaderboard
			addLeaderboard ();
		}

		public PlotModel CreatePlotModel (string username)
		{
			//Sets the graph information
			var plotModel = new PlotModel {
				Title = "Top 10 Mad Minutes:",
				LegendTitle = " Correct Answers ",
				LegendBorderThickness = 0.1,
				LegendBackground = OxyColors.Green,
				IsLegendVisible = true };

			var today = DateTime.Now;
			var past = DateTime.Now.AddDays (-10);

			var minValue = DateTimeAxis.ToDouble (past);
			var maxValue = DateTimeAxis.ToDouble (today);

			plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Bottom, Maximum = 10, Minimum = 0 });
			plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Left, Maximum = 100, Minimum = 0 });

			//Create two lines the first one for the correct answers and the second one for the incorrect answers
			var correct = new LineSeries {
				MarkerType = MarkerType.Circle,
				MarkerSize = 4,
				MarkerStroke = OxyColors.Green
			};

			var incorrect = new LineSeries {
				MarkerType = MarkerType.Circle,
				MarkerSize = 4,
				MarkerStroke = OxyColors.Red,
			};

			//Retrieve from the database the values to show the player
			ConnectToDatabase newConnection = new ConnectToDatabase ();
			ArrayList returnDatabase = newConnection.retrieveMadMinute (username);

			//For each value return we are going to plot a point
			for (int i = 0; i < returnDatabase.Count; i++) {
				string[] contentArray = (string [])returnDatabase [i];
				//Check if the date is correctly retrieved
				correct.Points.Add (new DataPoint (Convert.ToDouble(i), Convert.ToDouble(contentArray[0])));
				incorrect.Points.Add (new DataPoint (Convert.ToDouble(i), Convert.ToDouble(contentArray[1])));
			}

			//Add the points into the graph
			plotModel.Series.Add (correct);
			plotModel.Series.Add (incorrect);

			return plotModel;
		}

		public void addLeaderboard ()
		{
			//Find view that will contain leaderboard information
			var leaderboardArea = FindViewById<LinearLayout> (Resource.Id.leaderboards);

			int timeframe = 0; //Set how far back you want to go in days. 0 means all-time highest
			int score;

			//Connect to database
			SqlConnectionStringBuilder builder = ConnString.Builder;
            using (SqlConnection connection = new SqlConnection (builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder ();
				//Use procedure to retrieve highest scored from database
				sb.Append($"EXEC usp_RetrieveMadMinuteLeaderboard {timeframe}");
                string query = sb.ToString ();
				SqlCommand cmd = new SqlCommand (query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader ())
                {
					//Fill the leaderboard view with each high score
                    int rank = 1;
                    while (reader.Read())
                    {
						//Programmatically create a new textView that will hold high score information
						var newrow = createNewRow ();
						score = (int)reader ["Score"];
                        newrow.Text = $"{rank}. {score}";
						//Add newly created field to the leaderboard view
                        leaderboardArea.AddView(newrow);
                        rank++;
                    }
                }
                connection.Close();
            }
		}

		public TextView createNewRow ()
		{
			//Generate a new TextView and style it
			var newrow = new TextView (this);
			newrow.SetTextSize (Android.Util.ComplexUnitType.Dip, 25f);
			newrow.SetPadding (30, 10, 30, 10);
			newrow.SetBackgroundResource (Resource.Drawable.border);
			return newrow;
		}
	}
}



