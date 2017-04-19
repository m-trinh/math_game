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
	[Activity (Label = "Statistics", MainLauncher = false)]
	public class GenerateStats : Activity
	{
		
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Stats);

			PlotView view = FindViewById<PlotView> (Resource.Id.plot_view);
			string username = Intent.GetStringExtra ("UserName");
			username = "Ignacio";
			view.Model = CreatePlotModel(username);
			/*
			 * 
			ConnectToDatabase newConnection = new ConnectToDatabase();
			ArrayList returnDatabase = newConnection.retrieveMadMinute(username);
			int [] results = returnDatabase.ToArray (typeof (int)) as int[];
			var chart = new BarChartView(this)
			{
				ItemsSource = Array.ConvertAll(results, v => new BarModel { Value = v})
			};

			AddContentView(chart, new ViewGroup.LayoutParams (
			ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));*/

		}
		public PlotModel CreatePlotModel (string username)
		{
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

			ConnectToDatabase newConnection = new ConnectToDatabase ();
			ArrayList returnDatabase = newConnection.retrieveMadMinute (username);

			for (int i = 0; i < returnDatabase.Count; i++) {
				string[] contentArray = (string [])returnDatabase [i];
				//Check if the date is correctly retrieved
				correct.Points.Add (new DataPoint (Convert.ToDouble(i), Convert.ToDouble(contentArray[0])));
				incorrect.Points.Add (new DataPoint (Convert.ToDouble(i), Convert.ToDouble(contentArray[1])));
			}

			plotModel.Series.Add (correct);
			plotModel.Series.Add (incorrect);

			return plotModel;
		}
	}
}



