
using Android.App;
using Android.Text.Method;
using Android.Widget;
using Android.OS;
using System;
using System.Timers;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Content;
using Xamarin.Facebook.Login;

namespace WritePadXamarinSample
{
	[Activity (Label = "WritePadXamarinSample")]
	public class MadMinute : Activity
	{
		private LinearLayout topLayerCount;
		private FrameLayout containerLayer;
		private bool endTime = false;
		private TextView countDownStart;
		private TextView countdownView;
		private int countVariable;
		private Timer timer;
		private TextView ReadyGoStop;
		private Button button;
		private TextView questions_math;
		private TextView readyText;
		private InkView inkView;
		private Button submitAnswer;
		private int totalScore = 0;
		private int totalQuestions = 0;
		private TextView finalTotalScore;
		private Button replayGame;
		private TextView countDownView;
		private Button goBack;
		private string username;

		protected override void OnPause ()
		{
			base.OnDestroy ();
			LoginManager.Instance.LogOut ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			WritePadAPI.recoFree ();
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MadMinute);

			//Get the username
			username = Intent.GetStringExtra ("UserName");


			WritePadAPI.recoInit (BaseContext);
			WritePadAPI.initializeFlags (BaseContext);

			button = FindViewById<Button> (Resource.Id.RecognizeButton);
			inkView = FindViewById<InkView> (Resource.Id.ink_view);
			readyText = FindViewById<TextView> (Resource.Id.ready_text);
			topLayerCount= FindViewById<LinearLayout> (Resource.Id.topLayerCount);
			containerLayer = FindViewById<FrameLayout> (Resource.Id.containerLayer);
			countdownView = FindViewById<TextView> (Resource.Id.countdownStart);
			ReadyGoStop = FindViewById<TextView> (Resource.Id.ReadyGoStop);
			questions_math = FindViewById<TextView> (Resource.Id.questions_math);
			submitAnswer = FindViewById<Button> (Resource.Id.submitAnswer);
			finalTotalScore = FindViewById<TextView> (Resource.Id.finalTotalScore);
			replayGame = FindViewById<Button> (Resource.Id.replayGame);
			countDownView = FindViewById<TextView> (Resource.Id.time_countdown);
			goBack = FindViewById<Button> (Resource.Id.goBack);


			countVariable = 3;
			//calculateTime (countVariable);

			topLayerCount.Visibility = Android.Views.ViewStates.Invisible;
			if (endTime) {
				return;
			}
			ReadyGoStop.Text = "GO!";
			int minuteTime = 60;
			StartCountdownTimer (minuteTime);
			bool rightAnswer = false;
			ShowQuestions (rightAnswer);

			readyText.MovementMethod = new ScrollingMovementMethod ();
			//readyTextTitle.Text = Resources.GetString (Resource.String.Title) + " (" + WritePadAPI.getLanguageName () + ")";

			button.Click += delegate {
				readyText.Text = inkView.Recognize (false);
			};

			goBack.Click += delegate {
				var activity2 = new Intent (this, typeof (Activity2));
				activity2.PutExtra ("UserName", username);
				//activity2.PutExtra ("UserEmail", e.mProfile.Email);
				StartActivity (activity2);
			};

			replayGame.Click += delegate {
				replayGame.Enabled = false;
				restartSettings ();
			};

			/* WritePad SDK recognizing what the user wrote.*/
			inkView.OnReturnGesture += () => readyText.Text = inkView.Recognize (true);
			inkView.OnReturnGesture += () => inkView.cleanView (true);
			inkView.OnCutGesture += () => inkView.cleanView (true);
			var clearbtn = FindViewById<Button> (Resource.Id.ClearButton);
			clearbtn.Click += delegate {
				readyText.Text = "";
				inkView.cleanView (true);
			};
		}

		public void restartSettings ()
		{
			inkView.cleanView (true);
			topLayerCount.Visibility = Android.Views.ViewStates.Invisible;
			totalScore = 0;
			totalQuestions = 0;
			ReadyGoStop.Text = "Go!";
			finalTotalScore.Text = "Score: 0";
			countDownView.Text = "60";
			int minuteTime = 60;
			StartCountdownTimer (minuteTime);
			bool rightAnswer = false;
			ShowQuestions (rightAnswer);
		}

		private void ShowQuestions (bool rightAnswer)
		{
			RandomQuestions newQuestion = new RandomQuestions();


			questions_math.Text = newQuestion.FirstOperand.ToString () + " " + newQuestion.Operand + " " + newQuestion.SecondOperand.ToString ();

			submitAnswer.Click += (sender, e) => {
				totalQuestions++;
				readyText.Text = inkView.Recognize (false);
				rightAnswer = validateAnswer (newQuestion);
				if(rightAnswer)
					newQuestion = createNewQuestion ();
			};
		}

		private RandomQuestions createNewQuestion () {
			questions_math.SetBackgroundColor (Color.AliceBlue);
			RandomQuestions newQuestion = new RandomQuestions ();

			questions_math.Text = newQuestion.FirstOperand.ToString () + " " + newQuestion.Operand + " " + newQuestion.SecondOperand.ToString ();
			return newQuestion;
		}


		private bool validateAnswer (RandomQuestions question)
		{
			try {
				string[] answer = readyText.Text.Split ('\n');
				//If the user types 11 but the console reads like 1  1 
				string finalSolution = System.Text.RegularExpressions.Regex.Replace (answer[0], @"\s+", "");
				int checkAnswer = Int32.Parse (finalSolution);
				if (checkAnswer == question.Solution) {
					ReadyGoStop.Text = "CORRECT!";
					inkView.cleanView (true);
					totalScore++;
					finalTotalScore.Text = totalScore.ToString();
					questions_math.SetBackgroundColor (Color.Green);
					return true;
				} else {
					ReadyGoStop.Text = "TRY AGAIN!";
					return false;
				}
			} catch (FormatException) {
				return false;
			}
		}

		protected void calculateTime (int countVariable)
		{
			timer = new Timer ();
			timer.Interval = 1000;
			timer.Elapsed += Timer_Elapsed;
			timer.Start ();
			return;

		}

		private void Timer_Elapsed (object sender, ElapsedEventArgs e)
		{
			
			if (countVariable > 0) {
				RunOnUiThread (() => countdownView.Text = countVariable.ToString ());
				countVariable--;
			} else {

				containerLayer.RemoveView (topLayerCount);
				timer.Stop ();
				return;
			}
		}

		private async void StartCountdownTimer (int startingVal)
		{
			while (startingVal >= 0) {
				RunOnUiThread (() => countDownView.Text = startingVal.ToString ());
				startingVal = startingVal - 1;
				await Task.Delay (1000);
			}

			if (startingVal < 0) {
				ReadyGoStop.Text = "STOP!";
				topLayerCount.Visibility = Android.Views.ViewStates.Visible;
				countdownView.Text = "Time is up! Final Score: " + totalScore + " points";

				replayGame.Enabled = true;
				//Show the layer to say it is done

				return;
			}
		}

	}

	public class RandomQuestions{

		private int firstOperand;
		private int secondOperand;
		private string operand;

		public RandomQuestions () {
			Random random = new Random ();
			firstOperand = random.Next (0, 9);
			secondOperand = random.Next (0, 9);
			string [] operands = { "+", "-" };
			operand = operands [random.Next (0, operands.Length - 1)];//Only using addition

		}

		public int FirstOperand{
			get{
				return firstOperand;
			}
		}
		public int SecondOperand {
			get {
				return secondOperand;
			}
		}

		public string Operand {
			get {
				return operand;
			}
		}


		public int Solution {
			get {
				//The solution is the first operand the calculate given the operand and the second operator
				switch (Operand) {
					case "+":
						return FirstOperand + SecondOperand;
					case "-":
						return FirstOperand - SecondOperand;
					case "*":
						return FirstOperand * SecondOperand;
					case "/":
						return FirstOperand / SecondOperand;
				}
				return 0;
			}
		}

	}

}
