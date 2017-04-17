using Android.App;
using Android.Widget;
using Android.Graphics;

namespace WritePadXamarinSample
{
	[Activity (Label = "ChallengeQuestionActivity")]
	public class ChallengeQuestionActivity : QuestionActivity
	{
		public override void chooseResultView ()
		{
			SetContentView (Resource.Layout.SATChallengeResults);
		}

		public override int showStars (int correct)
		{
			var stars = FindViewById<ImageView> (Resource.Id.starResult);
			var resultMessage = FindViewById<TextView> (Resource.Id.resultMessage);

			int result = 0;

            if (correct >= 10)
            {
                stars.SetImageResource(Resource.Drawable.stars5);
                resultMessage.Text = "Perfect!";
				result = 5;
            }
            else if (correct >= 8)
            {
                stars.SetImageResource(Resource.Drawable.stars4);
                resultMessage.Text = "Great";
				result = 4;
            }
            else if (correct >= 6)
            {
                stars.SetImageResource(Resource.Drawable.stars3);
                resultMessage.Text = "Good";
				result = 3;
            }
            else if (correct >= 4)
            {
                stars.SetImageResource(Resource.Drawable.stars2);
                resultMessage.Text = "Getting There";
				result = 2;
            }
            else if (correct >= 2)
            {
                stars.SetImageResource(Resource.Drawable.stars1);
                resultMessage.Text = "Give It Another Shot";
				result = 1;
            }
            else
            {
                stars.SetImageResource(Resource.Drawable.stars0);
                resultMessage.Text = "Don't Give Up!";
            }

			return result;
		}

		public override int calculateCorrect ()
		{
			int correct = 0;

			TextView [] results = new TextView [questions.Count];
			results [0] = FindViewById<TextView> (Resource.Id.result1);
			results [1] = FindViewById<TextView> (Resource.Id.result2);
			results [2] = FindViewById<TextView> (Resource.Id.result3);
			results [3] = FindViewById<TextView> (Resource.Id.result4);
			results [4] = FindViewById<TextView> (Resource.Id.result5);
			results [5] = FindViewById<TextView> (Resource.Id.result6);
			results [6] = FindViewById<TextView> (Resource.Id.result7);
			results [7] = FindViewById<TextView> (Resource.Id.result8);
			results [8] = FindViewById<TextView> (Resource.Id.result9);
			results [9] = FindViewById<TextView> (Resource.Id.result10);

			for (int i = 0; i < individualResults.Length; i++) {
				if (individualResults [i]) {
					correct++;
					results [i].Text = "Correct";
					results [i].SetBackgroundColor (Color.Green);
				} else {
					results [i].Text = "Wrong";
					results [i].SetBackgroundColor (Color.Red);
				}
			}

			return correct;
		}
	}
}
