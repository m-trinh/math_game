using System;
using System.Data.SqlClient;
namespace WritePadXamarinSample
{
	public class ConnString
	{
		private static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder ();
		static ConnString ()
		{
            builder.DataSource = "teamred.database.windows.net";
           	builder.UserID = "teamredadmin";
           	builder.Password = "c$503teamred";
           	builder.InitialCatalog = "TeamRedMath";
		}

		public static SqlConnectionStringBuilder Builder 
		{
			get 
			{
				return builder;
			}
		}
	}
}
