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
			SqlConnectionStringBuilder builder = ConnString.Builder;
			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"EXEC dbo.usp_InsertIntoMadMinuteHistory '{User.username}', {correctGuesses}, {incorrectGuesses}, '{User.location}' ");
				String sql = sb.ToString ();

				using (SqlCommand command = new SqlCommand (sql, connection)) {
					int average = (int) command.ExecuteScalar();
					User.average = average;
				}
				connection.Close ();
			}

		} catch (SqlException e) {
			string exception = e.ToString();
			return false;
		} 

		return true;
	}

	public bool increaseExperience (int correntGuesses)
	{
		try {
			SqlConnectionStringBuilder builder = ConnString.Builder;
			using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"Exec usp_IncreaseUserExperience {User.username}, {correntGuesses * 25}");
				String sql = sb.ToString ();

				SqlCommand command = new SqlCommand (sql, connection);
				using (SqlDataReader reader = command.ExecuteReader ()) 
				{
					while (reader.Read ())
					{
						if ((int)reader ["USER_LEVEL"] > User.level) {
							User.hints = User.hints + ((int)reader ["USER_LEVEL"] - User.level);
							updateHint ();
						}

						User.level = (int)reader ["USER_LEVEL"];
						User.experience = (int)reader ["EXPERIENCE"];
					}
				}
				connection.Close ();
			}
		} catch (SqlException e) {
			string exception = e.ToString ();
			return false;
		} 
		return true;
	}

	public bool updateHint ()
	{
		try {
				SqlConnectionStringBuilder builder = ConnString.Builder;
				using (SqlConnection connection = new SqlConnection (builder.ConnectionString)) {
				connection.Open ();
				StringBuilder sb = new StringBuilder ();
				sb.Append ($"UPDATE USER_ACCESS SET HINTS = {User.hints} WHERE USERNAME = '{User.username}'");
				string query = sb.ToString ();
				SqlCommand cmd = new SqlCommand (query, connection);
				cmd.ExecuteNonQuery ();
				connection.Close ();
			}
		} catch (SqlException e) {
			string exception = e.ToString ();
			return false;
		}
		return true;
	}

	public bool saveUser ()
	{
		try {
			SqlConnectionStringBuilder builder = ConnString.Builder;

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

				sb.Append($"execute usp_Login '{User.username}','{User.email}',null,{1},'{User.firstName}','{User.lastName}','{User.location}'");
				String sql = sb.ToString ();

				using (SqlCommand command = new SqlCommand (sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader()) {
						while (reader.Read ()) {
							//Returns
							//username: username, first name: first_name, level: user_level, experience: experience and location: user_location
							User.level = (int)reader ["USER_LEVEL"];
							User.experience = (int)reader ["EXPERIENCE"];
							User.hints = (int)reader ["HINTS"];
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
			SqlConnectionStringBuilder builder = ConnString.Builder;

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
