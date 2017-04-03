using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
//using System.Drawing;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace WritePadXamarinSample
{
    [Activity(Label = "QuestionActivity")]
    public class QuestionActivity : Activity
    {
		Random random = new Random ();
		List<Question> questions = new List<Question> ();
		int currentQuestion = 0;
		int correct = 0;
		Button [] answerButtons;
		int correctAns;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Question);

			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder ();
			builder.DataSource = "teamred.database.windows.net";
			builder.UserID = "teamredadmin";
			builder.Password = "c$503teamred";
			builder.InitialCatalog = "TeamRedMath";

			using (SqlConnection conn = new SqlConnection (builder.ConnectionString)) {
				conn.Open ();
				string query = "SELECT * FROM SAT_QUESTIONS WHERE DIFFICULTY = 'EASY' AND CATEGORY = 'ALGEBRA' AND END_DATE IS NULL";
				SqlCommand cmd = new SqlCommand (query, conn);
				using (SqlDataReader reader = cmd.ExecuteReader ()) 
				{
					while (reader.Read ()) 
					{
						questions.Add (new Question ((int)reader ["ROW_ID"], reader ["QUESTION"] as Byte [], reader ["CORRECT_ANSWER"] as string, reader ["INCORRECT_ONE"] as string, reader ["INCORRECT_TWO"] as string, reader ["INCORRECT_THREE"] as string));
					}
				}

				conn.Close ();
			}

			populateButtonsArray ();
			questions = shuffleList (questions);
			showQuestion (questions [currentQuestion]);
        }

		public void showQuestion (Question question)
		{
			if (currentQuestion > 0) 
			{
				answerButtons [correctAns].Click -= incrementCorrect; 
			}

			currentQuestion++;
			Console.WriteLine (currentQuestion);

			var questionImage = FindViewById<ImageView> (Resource.Id.questionView);

			byte [] imageByte = question.QuestionImage;
			var image = BitmapFactory.DecodeByteArray (imageByte, 0, imageByte.Length);
			var metrics = Resources.DisplayMetrics;
			var width = metrics.WidthPixels - 20;
			var height = image.Height * width / image.Width;
			questionImage.SetImageBitmap (Bitmap.CreateScaledBitmap (image, width, height, false));

			string [] wrongAns = new string [] {question.Wrong1, question.Wrong2, question.Wrong3};
			correctAns = random.Next (0, 4);
			int wrongAnsIndex = 0;

			for (int i = 0; i < answerButtons.Length; i++) 
			{
				if (i == correctAns) 
				{
					RunOnUiThread (() => answerButtons [correctAns].Text = (question.Correct));
					answerButtons [correctAns].Click += incrementCorrect;
				}
				else 
				{
					RunOnUiThread (() => answerButtons [i].Text = (wrongAns[wrongAnsIndex]));
					wrongAnsIndex++;
				}

				if (currentQuestion == questions.Count - 1) 
				{
					answerButtons [i].Click -= nextQuestion;
					answerButtons [i].Click += delegate 
					{
						StartActivity (typeof (Activity3));
					};
				}
			}
		}

		public List<Question> shuffleList (List<Question> list)
		{
			int randomInt;
			for (int i = list.Count - 1; i >= 0; i--) 
			{
				randomInt = random.Next (0, list.Count);
				Question temp = list [randomInt];
				list [randomInt] = list [i];
				list [i] = temp;
			}
			return list;
		}

		public void populateButtonsArray ()
		{
			var buttonA = FindViewById<Button> (Resource.Id.answerA);
			var buttonB = FindViewById<Button> (Resource.Id.answerB);
			var buttonC = FindViewById<Button> (Resource.Id.answerC);
			var buttonD = FindViewById<Button> (Resource.Id.answerD);

			answerButtons = new Button [] { buttonA, buttonB, buttonC, buttonD };

			for (int i = 0; i < answerButtons.Length; i++) 
			{
				answerButtons [i].Click += nextQuestion;
			}
		}

		void incrementCorrect (object sender, EventArgs ea)
		{
			correct++;
		}

		void nextQuestion (object sender, EventArgs ea)
		{
			showQuestion (questions [currentQuestion]);
		}
    }
}