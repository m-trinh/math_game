using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Http;
using Plugin.Geolocator;
using System.Threading.Tasks;

namespace WritePadXamarinSample
{
	public class LocationFinder
	{
		public LocationFinder()
		{
		}

		public async Task findPosition()
		{

			//Instantiate locator and set desired accuracy
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;

			// Get the current device position. Leave it null if geo-location is disabled,
			// return position (0, 0) if unable to acquire.
			if (locator.IsGeolocationEnabled)
			{
				// Allow ten seconds for geo-location determination.                    
				var position = await locator.GetPositionAsync(10000);
				string latitude = position.Latitude.ToString();
				string longitude = position.Longitude.ToString();

				//Task<string> jsonTask = DownloadDataAsync(latitude, longitude);

				//Set requestURL to call Google API with Lat and Lon from Android app
				string requestURL = String.Concat("https://maps.googleapis.com/maps/api/geocode/json?latlng=", latitude, ",", longitude, "&key=AIzaSyD1GYcDejuONv-WDSZekARsb48SPc2cwCs");
				//string requestURL = String.Concat("https://maps.googleapis.com/maps/api/geocode/json?latlng=42.575001,-70.932122&key=AIzaSyD1GYcDejuONv-WDSZekARsb48SPc2cwCs");


				var httpClient = new HttpClient();
				var response = httpClient.PostAsync(requestURL, new StringContent("")).Result;

				//Store the result of the GOogle API call in a string
				string json = response.Content.ReadAsStringAsync().Result;

				//bind the String to GoogleResults using JsonConvert
				GoogleResults google = JsonConvert.DeserializeObject<GoogleResults>(json.ToString());

				if (google != null && google.results.Count >= 4)
					User.location = google.results[3].formatted_address;
				else
					User.location = "Not Working";

				//ADD CONNECTION STRING INFO HERE
				//
				//
				//
				/*string connectionString = "ENTER QUERY STRING HERE";
					//
					//
					//
					//

					using (SqlConnection conn = new SqlConnection(connectionString))
					{
						conn.Open();
						using (SqlCommand sql = new SqlCommand($"Insert into MAD_MINUTE_HISTORY ([USERNAME],[CORRECT_GUESSES],[INCORRECT_GUESSES],[CREATE_DATE],[CATEGORY],[LOCATION]) VALUES('Jojos',1,2,GETDATE(),'Test','{google.results[3].formatted_address}')", conn))
						{
							sql.ExecuteNonQuery();
						}

		            	//CLose the connection to the DB completely
		            	conn.Close();
					}*/
			}
			else
			{
				//Do Nothing
			}

		}
		public class AddressComponent
		{
			public string long_name { get; set; }
			public string short_name { get; set; }
			public List<string> types { get; set; }
		}

		public class Location
		{
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class Northeast
		{
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class Southwest
		{
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class Viewport
		{
			public Northeast northeast { get; set; }
			public Southwest southwest { get; set; }
		}

		public class Northeast2
		{
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class Southwest2
		{
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class Bounds
		{
			public Northeast2 northeast { get; set; }
			public Southwest2 southwest { get; set; }
		}

		public class Geometry
		{
			public Location location { get; set; }
			public string location_type { get; set; }
			public Viewport viewport { get; set; }
			public Bounds bounds { get; set; }
		}

		public class GoogleResult
		{
			public List<AddressComponent> address_components { get; set; }
			public string formatted_address { get; set; }
			public Geometry geometry { get; set; }
			public string place_id { get; set; }
			public List<string> types { get; set; }
		}

		public class GoogleResults
		{
			public List<GoogleResult> results { get; set; }
			public string status { get; set; }
		}

	}
}
