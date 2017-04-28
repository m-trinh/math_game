using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using System.Data.SqlClient;
using System.Diagnostics;
using Android.Views;

namespace WritePadXamarinSample
{
    [Activity(Label = "QuestionActivity")]
    public class QuestionActivity : Activity
    {
		Random random = new Random ();
        private SqlConnectionStringBuilder builder = ConnString.Builder;
        private string username;
		private string category;
		private string difficulty;
		protected List<Question> questions = new List<Question> ();
		private int currentQuestion = 0;
		private Button [] answerButtons;
		private int correctAns;
		private Stopwatch stopwatch = new Stopwatch ();
		private double elapsedTime;
        protected bool[] individualResults;
		private int hints = 10;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameReady);
			          
            IList<string> details = Intent.GetStringArrayListExtra("details");
            username = details[0];
            category = details[1];
            difficulty = details[2];

            questions = getQuestions();
            var readyButton = FindViewById<Button>(Resource.Id.ready);
            readyButton.Click += startGame; 
        }

		public List<Question> getQuestions ()
		{
			List<Question> results = new List<Question>();

			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"EXEC dbo.usp_GetSAT_Questions '{category}', {difficulty}, {username}, 0");
				string query = sb.ToString();
				SqlCommand cmd = new SqlCommand (query, connection);
				using (SqlDataReader reader = cmd.ExecuteReader ()) 
				{
					while (reader.Read ()) 
					{
						results.Add (new Question ((int)reader ["ROW_ID"], reader ["QUESTION"] as Byte [], reader ["CORRECT_ANSWER"] as string, reader ["INCORRECT_ONE"] as string, reader ["INCORRECT_TWO"] as string, reader ["INCORRECT_THREE"] as string));
					}
				}
				connection.Close ();
			}

			return results;
		}

		public void showQuestion (Question question)
		{
			if (currentQuestion > 0) 
			{
				for (int i = 0; i < answerButtons.Length; i++) 
				{
					answerButtons [i].Enabled = true;
					if (i == correctAns) {
						answerButtons [correctAns].Click -= sendCorrect;
					} 
					else 
					{
						answerButtons [i].Click -= sendIncorrect;
					}
				}
			}

			var questionImage = FindViewById<ImageView> (Resource.Id.questionView);
			var hintButton = FindViewById<Button> (Resource.Id.hint);
			hintButton.Enabled = true;

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
					answerButtons [correctAns].Click += sendCorrect;
				}
				else 
				{
					RunOnUiThread (() => answerButtons [i].Text = (wrongAns[wrongAnsIndex]));
					wrongAnsIndex++;
					answerButtons [i].Click += sendIncorrect;
				}

				if (currentQuestion == questions.Count - 2) 
				{
					answerButtons [i].Click -= nextQuestion;
					answerButtons [i].Click += showResults;
				}
			}
			currentQuestion++;
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

		//public override Dialog onCreateDialog (Bundle savedInstanceState)
		//{
		//	AlertDialog.Builder builder = new AlertDialog.Builder (this);
		//	var equationsView = LayoutInflater.Inflate (Resource.Layout.Equations, null);
		//	builder.SetTitle("Equations");
  //          builder.SetView(equationsView);

		//	builder.SetPositiveButton("Okay", delegate { builder.Dispose(); });
  //          builder.Show();
		//}

        void startGame(object sender, EventArgs ea)
        {
            SetContentView(Resource.Layout.Question);

            individualResults = new bool[questions.Count];
            populateButtonsArray();
            stopwatch.Start();
            showQuestion(questions[currentQuestion]);

			var equationButton = FindViewById<Button> (Resource.Id.equations);
			equationButton.Click += delegate
            {
				var equationsView = LayoutInflater.Inflate (Resource.Layout.Equations, null);
				var equationImage = equationsView.FindViewById<ImageView> (Resource.Id.equationsImage);
				equationImage.SetImageResource (Resource.Drawable.equations);
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle("Equations");
	            builder.SetView(equationsView);

				builder.SetPositiveButton("Okay", delegate { builder.Dispose(); });
	            builder.Show();
            };

			var hintButton = FindViewById<Button> (Resource.Id.hint);
			hintButton.Click += delegate {
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("Use a Hint");
				builder.SetMessage ($"You have {hints} hints left. Do you want to use one to eliminate an answer?");
				builder.SetPositiveButton ("Okay", delegate { useHint (); hintButton.Enabled = false; builder.Dispose (); });
				builder.SetNegativeButton ("Cancel", delegate { builder.Dispose (); });
				builder.Show ();
			};
        }

		public void useHint ()
		{
			int remove = random.Next (0, 3);

			while (remove == correctAns) 
			{
				remove = random.Next (0, 3);
			}

			answerButtons [remove].Enabled = false;
			hints--;
		}

		void sendCorrect (object sender, EventArgs ea)
		{
            individualResults[currentQuestion] = true;
			sendData (1);
		}

		void sendIncorrect (object sender, EventArgs ea)
		{
            individualResults[currentQuestion] = false;
			sendData (0);
		}

		void nextQuestion (object sender, EventArgs ea)
		{
			showQuestion (questions [currentQuestion]);
		}

        void showResults(object sender, EventArgs ea)
        {
			chooseResultView();

			int correct = calculateCorrect ();

			int stars = showStars (correct);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append($"Exec usp_InsertIntoSAT_Stars_By_User {username}, {category}, {difficulty}, {stars}");
                string query = sb.ToString();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();

				sb.Clear ();
				sb.Append ($"Exec usp_IncreaseUserExperience {username}, {correct * 100}");
				query = sb.ToString ();
				cmd = new SqlCommand (query, connection);
				var result = cmd.ExecuteScalar ();
				connection.Close ();

                connection.Close();
            }

			var exp = FindViewById<TextView> (Resource.Id.experience);
			exp.Text = $"+{correct * 100} EXP";

            var finish = FindViewById<Button>(Resource.Id.finish);
            finish.Click += delegate
            {
                StartActivity(typeof(Activity2));
            };
        }

		public virtual void chooseResultView ()
		{
            SetContentView (Resource.Layout.SATResults);
		}

		public virtual int showStars (int correct)
		{
			var stars = FindViewById<ImageView> (Resource.Id.starResult);
			var resultMessage = FindViewById<TextView> (Resource.Id.resultMessage);

            if (correct >= 5)
            {
                stars.SetImageResource(Resource.Drawable.stars5);
                resultMessage.Text = "Perfect!";
            }
            if (correct == 4)
            {
                stars.SetImageResource(Resource.Drawable.stars4);
                resultMessage.Text = "Great";
            }
            if (correct == 3)
            {
                stars.SetImageResource(Resource.Drawable.stars3);
                resultMessage.Text = "Good";
            }
            if (correct == 2)
            {
                stars.SetImageResource(Resource.Drawable.stars2);
                resultMessage.Text = "Getting There";
            }
            if (correct == 1)
            {
                stars.SetImageResource(Resource.Drawable.stars1);
                resultMessage.Text = "Give It Another Shot";
            }
            else
            {
                stars.SetImageResource(Resource.Drawable.stars0);
                resultMessage.Text = "Don't Give Up!";
            }

			return correct;
		}

		public virtual int calculateCorrect ()
		{
			int correct = 0;

			TextView [] results = new TextView [questions.Count];
			results [0] = FindViewById<TextView>(Resource.Id.result1);
            results [1] = FindViewById<TextView>(Resource.Id.result2);
            results [2] = FindViewById<TextView>(Resource.Id.result3);
            results [3] = FindViewById<TextView>(Resource.Id.result4);
            results [4] = FindViewById<TextView>(Resource.Id.result5);

            for (int i = 0; i<individualResults.Length; i++)
            {
                if (individualResults [i])
                {
                    correct++;
                    results [i].Text = "Correct";
                    results [i].SetBackgroundColor (Color.Green);
                }
                else
                {
                    results [i].Text = "Wrong";
                    results [i].SetBackgroundColor (Color.Red);
                }
            }

			return correct;
		}

		public void sendData (int correct)
		{
			elapsedTime = stopwatch.Elapsed.TotalSeconds;
			stopwatch.Restart();
			SqlConnectionStringBuilder builder = ConnString.Builder;
			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) 
			{
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"Exec usp_InsertIntoSAT_History {questions[currentQuestion].Row_id}, {username}, {correct}, {elapsedTime}");
				string query = sb.ToString ();
				SqlCommand cmd = new SqlCommand (query, connection);
				cmd.ExecuteNonQuery ();
			}
		}

		//public List<Question> shuffleList (List<Question> list)
		//{
		//	int randomInt;
		//	for (int i = list.Count - 1; i >= 0; i--) 
		//	{
		//		randomInt = random.Next (0, list.Count);
		//		Question temp = list [randomInt];
		//		list [randomInt] = list [i];
		//		list [i] = temp;
		//	}
		//	return list;
		//}
    }
}
