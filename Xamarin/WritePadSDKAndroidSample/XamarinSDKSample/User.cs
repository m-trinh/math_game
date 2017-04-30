using System.Data.SqlClient;
using System.Text;

namespace WritePadXamarinSample
{
	public static class User
	{
		public static string username;
		public static string firstName;
		public static string lastName;
		public static string location = "Fenway/Kenmore, Boston, MA, USA";
		public static int level;
		public static int experience;
		public static string email;
		public static int average;
		public static int hints;

		public static void login ()
		{
			SqlConnectionStringBuilder builder = ConnString.Builder;			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"Select * from USER_ACCESS where username = '{username}'");
				string query = sb.ToString ();
				SqlCommand cmd = new SqlCommand (query, connection);
				using (SqlDataReader reader = cmd.ExecuteReader ()) 
				{
					while (reader.Read ())
					{
						username = (string)reader ["USERNAME"];
						firstName = (string)reader ["FIRST_NAME"];
						lastName = (string)reader ["LAST_NAME"];
						level = (int)reader ["USER_LEVEL"];
						experience = (int)reader ["EXPERIENCE"];
						hints = (int)reader ["HINTS"];
					}
				}
				connection.Close ();
			}
		}
	}
}

