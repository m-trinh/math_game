using Android.App;
using Android.OS;
using System;
using Android.Views;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using System.Collections;

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
			string username = Intent.GetStringExtra ("UserName");

			//Create the View
			view.Model = CreatePlotModel(username);

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
	}
}



