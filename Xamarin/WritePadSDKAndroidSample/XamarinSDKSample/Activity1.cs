
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
	/**
	 * Class Activity1:
	 * 	Allows the user to train with easy algebraic operations.
	 * 	It is done so the user gets confident with writepad.
	 * 	There is no time control.
	 *	The score won't be saved in the database.
	*/
	[Activity(Label = "WritePadXamarinSample")]
	public class Activity1 : Activity
	{
		/*
			Defining the variables in the class
		*/
		private LinearLayout topLayerCount;
		private FrameLayout containerLayer;
		private bool endTime = false;
		private TextView countDownStart;
		private TextView countdownView;
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
		private string username = User.username;

		/*
		 * OnPause()
		 * Log out the user
		*/
		protected override void OnPause()
		{
			base.OnDestroy();
			LoginManager.Instance.LogOut();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			WritePadAPI.recoFree();
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Load the Training View
			SetContentView(Resource.Layout.Training);

			//Get the username from the home activity.
			//The values are passed as string
			username = Intent.GetStringExtra("UserName");

			//Initialize the WritePad API
			WritePadAPI.recoInit(BaseContext);
			WritePadAPI.initializeFlags(BaseContext);

			//Initialize the variables
			button = FindViewById<Button>(Resource.Id.RecognizeButton);
			inkView = FindViewById<InkView>(Resource.Id.ink_view);
			readyText = FindViewById<TextView>(Resource.Id.ready_text);
			containerLayer = FindViewById<FrameLayout>(Resource.Id.containerLayer);
			countdownView = FindViewById<TextView>(Resource.Id.countdownStart);
			ReadyGoStop = FindViewById<TextView>(Resource.Id.ReadyGoStop);
			questions_math = FindViewById<TextView>(Resource.Id.questions_math);
			submitAnswer = FindViewById<Button>(Resource.Id.submitAnswer);
			finalTotalScore = FindViewById<TextView>(Resource.Id.finalTotalScore);
			goBack = FindViewById<Button>(Resource.Id.goBack);

			//Starts the game!
			ReadyGoStop.Text = "GO!";
			bool rightAnswer = false;
			ShowQuestions(rightAnswer);

			readyText.MovementMethod = new ScrollingMovementMethod();
			//readyTextTitle.Text = Resources.GetString (Resource.String.Title) + " (" + WritePadAPI.getLanguageName () + ")";

			button.Click += delegate
			{
				readyText.Text = inkView.Recognize(false);
			};

			goBack.Click += delegate
			{
				var activity2 = new Intent(this, typeof(Activity2));
				activity2.PutExtra("UserName", username);
				//activity2.PutExtra ("UserEmail", e.mProfile.Email);
				StartActivity(activity2);
			};

			replayGame.Click += delegate
			{
				replayGame.Enabled = false;
				restartSettings();
			};

			/* WritePad SDK recognizing what the user wrote.*/
			inkView.OnReturnGesture += () => readyText.Text = inkView.Recognize(true);
			inkView.OnReturnGesture += () => inkView.cleanView(true);
			inkView.OnCutGesture += () => inkView.cleanView(true);
			var clearbtn = FindViewById<Button>(Resource.Id.ClearButton);
			clearbtn.Click += delegate
			{
				readyText.Text = "";
				inkView.cleanView(true);
			};
		}

		public void restartSettings()
		{
			inkView.cleanView(true);
			topLayerCount.Visibility = Android.Views.ViewStates.Invisible;
			totalScore = 0;
			totalQuestions = 0;
			ReadyGoStop.Text = "Go!";
			finalTotalScore.Text = "Score: 0";
			countDownView.Text = "60";
			bool rightAnswer = false;
			ShowQuestions(rightAnswer);
		}

		/**
		 * ShowQuestions: shows to the player a random question.
		 * @params param name="rightAnswer"
		 * 	If the user has a correct answer then true will be passed to the method.
		 * @postcondition:
		 * 	The method listens for the user to click the button submit.
		 * 	If the button is clicked then the answer is validated calling the method validadteAnswer().
		 * 	If the validation is true then show a new question
		 */
		private void ShowQuestions(bool rightAnswer)
		{
			RandomQuestion newQuestion = new RandomQuestion();


			questions_math.Text = newQuestion.FirstOperand.ToString() + " " + newQuestion.Operand + " " + newQuestion.SecondOperand.ToString();

			submitAnswer.Click += (sender, e) =>
			{
				totalQuestions++;
				readyText.Text = inkView.Recognize(false);
				rightAnswer = validateAnswer(newQuestion);
				if (rightAnswer)
					newQuestion = createNewQuestion();
			};
		}

		/**
		 * createNewQuestion:
		 * 	Generates a new RandomQuestion() class.
		 * @returns
		 * 	returns the new RandomQuestion() generated
		 * 	
		 */ 
		private RandomQuestion createNewQuestion()
		{
			questions_math.SetBackgroundColor(Color.AliceBlue);
			RandomQuestion newQuestion = new RandomQuestion();

			questions_math.Text = newQuestion.FirstOperand.ToString() + " " + newQuestion.Operand + " " + newQuestion.SecondOperand.ToString();
			return newQuestion;
		}

		/**
		 * ValidateAnswer: check that the answer introduced by the user is the correct one to the question
		 * @param name="question"
		 * 	Class randomQuestion that have the parameters to be able to check if the answer was correct or not.
		 * @returns
		 * 	Returns true if the answer was correct
		 * 	returns false if the answer was incorrect
		 */
		private bool validateAnswer(RandomQuestion question)
		{
			try
			{
				//Get only the first line of the options writepad returns
				string[] answer = readyText.Text.Split('\n');
				//If the user types 11 but the console reads like 1  1 
				string finalSolution = System.Text.RegularExpressions.Regex.Replace(answer[0], @"\s+", "");

				//As writeapad sometimes instead of recognising numbers recognises letters, this are the most common
				//mistakes writepad did.
				string[] badchars = new string[] { "l", "i", "I", "s", "S", " ", "}", "a", "g", "o", "O" };
				string[] goodchars = new string[] { "1", "1", "1", "5", "S", "", "3", "4", "6", "0", "0" };

				for (int i = 0; i<goodchars.Length; i++)
				{
					finalSolution = finalSolution.Replace(badchars[i], goodchars[i]);
				}

				int checkAnswer = Int32.Parse(finalSolution);
				//If the answer of the user is correct.
				if (checkAnswer == question.Solution)
				{
					//Show the user his answer was correct
					ReadyGoStop.Text = "CORRECT!";
					inkView.cleanView(true);
					totalScore++;
					finalTotalScore.Text = totalScore.ToString();
					questions_math.SetBackgroundColor(Color.Green);
					return true;
				}
				else
				{
					ReadyGoStop.Text = "TRY AGAIN!";
					return false;
				}
			}
			catch (FormatException)
			{
				return false;
			}
		}

	}

	/**
	 * RandomQuestion class generates a random question to the user
	 * firstOperand: is the first operand in the equation
	 * secondOperand: is the second operand in the equation
	 * operand: is the type of ecuation we want to apply to the operands
	 */ 
	public class RandomQuestion
	{

		private int firstOperand;
		private int secondOperand;
		private string operand;

		/**
		 * Constructor generates the random operands
		 */ 
		public RandomQuestion()
		{
			Random random = new Random();
			firstOperand = random.Next(0, 9);
			secondOperand = random.Next(0, 9);
			string[] operands = { "+", "-" };//Use or addition or substraction
			operand = operands[random.Next(0, operands.Length - 1)];//Only using addition

		}

		public int FirstOperand
		{
			get
			{
				return firstOperand;
			}
		}
		public int SecondOperand
		{
			get
			{
				return secondOperand;
			}
		}

		public string Operand
		{
			get
			{
				return operand;
			}
		}

		/**
		 * Calculates the solution of the random question generated.
		 */
		public int Solution
		{
			get
			{
				//The solution is the first operand the calculate given the operand and the second operator
				switch (Operand)
				{
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
