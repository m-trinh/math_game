
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
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Share.Model;

namespace WritePadXamarinSample
{
	/**
	 * 
	 * Mad Minute Game!
	 * 
	 * Get ready to answer as many questions as you can in under 1 minute.
	 * 
	 * The operations difficulty might increase given the number of correct or incorrect answers.
	 * 
	 * Once the game is finished you will have to share your results with your friends on facebook,
	 * replay to improve your previous score or try other amazing games!
	 * 
	 * The structure of this class is very similar to Activity1
	 * 
	 */ 
	[Activity (Label = "WritePadXamarinSample")]
	public class MadMinute : Activity
	{
		//Defining the variables
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
		private ShareButton sharingButton;

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

			//Initialize the variables with the componens of the View MadMinute
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
			sharingButton = FindViewById<ShareButton>(Resource.Id.shareButton);


			countVariable = 3;
			//calculateTime (countVariable);

			//The top layer at the beginning of the game doesn't display
			topLayerCount.Visibility = Android.Views.ViewStates.Invisible;
			if (endTime) {
				return;
			}
			//Show the user he has to start playing
			ReadyGoStop.Text = "GO!";
			//Initialize the time to 60seconds
			int minuteTime = 60;
			//Begins the countdown!
			StartCountdownTimer (minuteTime);
			bool rightAnswer = false;
			ShowQuestions (rightAnswer);

			readyText.MovementMethod = new ScrollingMovementMethod ();
			//readyTextTitle.Text = Resources.GetString (Resource.String.Title) + " (" + WritePadAPI.getLanguageName () + ")";

			//This button allows the player to check the answer before submiting.
			button.Click += delegate {
				readyText.Text = inkView.Recognize (false);
			};

			//This allows the player to share the results to his facebook page.
			ShareLinkContent content = new ShareLinkContent.Builder().
			                                               SetContentTitle("Mad Minute Mode! MathAttack Game").
			                                               SetContentDescription("I got " + totalScore + " answers right under a minute."+
			                                                                    " Can you beat me?").
			                                               Build();
			sharingButton.ShareContent = content;

			//Goes back to the menu of the game to play more games!
			goBack.Click += delegate {
				var activity2 = new Intent (this, typeof (Activity2));
				activity2.PutExtra ("UserName", username);
				//activity2.PutExtra ("UserEmail", e.mProfile.Email);
				StartActivity (activity2);
			};

			//The user wants to improve the previous score
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

		/**
		 * RestartSettings is called when the button replayGame has been clicked.
		 * @postcondition:
		 * 	All the variables are set to the predifined values.
		 */ 
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

				string[] badchars = new string[] { "l", "i", "I", "s", "S", " ", "}", "a", "g", "o", "O", "r", "f" };
				string[] goodchars = new string[] { "1", "1", "1", "5", "S", "", "3", "4", "6", "0", "0", "8", "8" };

				for (int i = 0; i < goodchars.Length; i++)
				{
					finalSolution = finalSolution.Replace(badchars[i], goodchars[i]);
				}

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

		/**
		 * StartCountdownTimer is an asyncronous function that counts the time left of the player.
		 * It had to be created as asyncronous because if not the app only would count down freezing the screen for the player.
		 * 
		 * @params startingVal:
		 * 	Is an integer that gives the amount of time the user has to play.
		 * 
		 * @Postcondition
		 * 	If the time is over, a top layer view will appear to the player, showing him the score and a menu.
		 * 
		 */ 
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
				//Add the share button with the results


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
