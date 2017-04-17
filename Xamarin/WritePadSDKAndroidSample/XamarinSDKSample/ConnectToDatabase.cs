using System;
using System.Collections.Generic;
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


}
