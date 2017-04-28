
using System;
using System.Collections;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data;
using System.Data.SqlClient;
using WritePadXamarinSample;

public class ConnectToDatabase
{
	public bool insertToMadMinute (string username, int correctGuesses, int incorrectGuesses)
	{

		try {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder ();
			builder.DataSource = "teamred.database.windows.net";
			builder.UserID = "teamredadmin";
			builder.Password = "c$503teamred";
			builder.InitialCatalog = "TeamRedMath";

			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"EXEC dbo.usp_InsertIntoMadMinuteHistory '{username}', {correctGuesses}, {incorrectGuesses}");
				String sql = sb.ToString ();

				using (SqlCommand command = new SqlCommand (sql, connection)) {
					using (SqlDataReader reader = command.ExecuteReader ()) {
						while (reader.Read ()) {
							Console.WriteLine ("{0} {1}", reader.GetString (0), reader.GetString (1));
						}
					}
				}
				connection.Close ();
			}

		} catch (SqlException) {
			return false;
		} 

		return true;
	}


	public bool saveUser (UserInfo user)
	{
		try {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource = "teamred.database.windows.net";
			builder.UserID = "teamredadmin";
			builder.Password = "c$503teamred";
			builder.InitialCatalog = "TeamRedMath";

			using (SqlConnection connection = new SqlConnection (builder.ConnectionString))
			{
				connection.Open();
				StringBuilder sb = new StringBuilder ();

				/*
				usp_Login {user.username},
				{user.email},
				{''},
				{1},
				{user.firstName},
				{user.lastName}*/
				sb.Append($"execute usp_Login '{user.FirstName+user.LastName}','{user.Email}',null,{1},'{user.FirstName}','{user.LastName}','Boston, MA'");
				String sql = sb.ToString ();

				using (SqlCommand command = new SqlCommand (sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader()) {
						while (reader.Read ()) {
							//Returns
							//username: username, first name: first_name, level: user_level, experience: experience and location: user_location
							Console.WriteLine ("");
						}
					}
				}
				connection.Close();
			}

		} catch (SqlException) {
			return false;
		}

		return false;
	}

	public ArrayList retrieveMadMinute(string username)
	{

		ArrayList read = new ArrayList();
		try
		{
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource = "teamred.database.windows.net";
			builder.UserID = "teamredadmin";
			builder.Password = "c$503teamred";
			builder.InitialCatalog = "TeamRedMath";

			using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
			{
				connection.Open();
				StringBuilder sb = new StringBuilder();
				sb.Append($"execute usp_RetrieveMadMinuteHistory {username}");
				String sql = sb.ToString();

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string [] values = new String[3];
							values [0] = reader [0].ToString();
							values [1] = (string)reader [1].ToString();
							string[] dates = reader [2].ToString ().Split (' ');
							values [2] = dates[0];
							read.Add(values);
						}
					}
				}
				connection.Close();
			}

		}
		catch (SqlException)
		{
			ArrayList wrong = new ArrayList();
			string [] incorrect = { "-1", "-2", "-1" };
			wrong.Add (incorrect);
			return wrong;
		}
		return read;
	}


}
