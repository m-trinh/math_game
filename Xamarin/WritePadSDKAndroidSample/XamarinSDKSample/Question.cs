using System;
namespace WritePadXamarinSample
{
	public class Question
	{
		public int Row_id {get; set;}
		public byte[] QuestionImage { get; set; }
		public string Correct { get; set; }
		public string Wrong1 { get; set; }
		public string Wrong2 { get; set; }
		public string Wrong3 { get; set; }
		public string Hint { get; set; }

		public Question (int row_id, byte[] questionImage, string correct, string wrong1, string wrong2, string wrong3)
		{
			Row_id = row_id;
			QuestionImage = questionImage;
			Correct = correct;
			Wrong1 = wrong1;
			Wrong2 = wrong2;
			Wrong3 = wrong3;
		}
	}
}
