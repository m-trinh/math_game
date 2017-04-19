using Android.App;

namespace WritePadXamarinSample
{
	public class UserInfo
	{
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set;}
		public string Email { get; set;}

		public UserInfo () {
		}
		public UserInfo (string firstName, string lastName, string name)
		{
			FirstName = firstName;
			LastName = lastName;
			Name = name;
		}
	}
}
