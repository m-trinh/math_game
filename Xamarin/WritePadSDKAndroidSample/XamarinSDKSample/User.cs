using System;
namespace WritePadXamarinSample
{
	public static class User
	{
		private static string firstName;
		static User (string firstName, string lastName, string email, int level, int experience)
		{
			User.firstName = firstName;
		}
	}
}
