using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;


namespace BusBoard.ConsoleApp
{
	class Program
	{
		static List<string> GetLatLong(string postcode)
			{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			var postcodeClient = new RestClient();
			postcodeClient.BaseUrl = new Uri("http://api.postcodes.io/");

			var postcodeRequest = new RestRequest();
			postcodeRequest.Resource = $"postcodes/{postcode}";

			var apiResponse = postcodeClient.Execute<PostcodeApiResponse>(postcodeRequest).Data;
			var postcodeData = apiResponse.result;

			List<string> latLong = new List<string>();
			latLong.Add(postcodeData.latitude);
			latLong.Add(postcodeData.longitude);

			return latLong;
		}

		static List<Stop> GetBusStopCodes(List<string> latLong)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			var tflClient = new RestClient();
			tflClient.BaseUrl = new Uri("https://api.tfl.gov.uk/");

			var stopsRequest = new RestRequest();
			stopsRequest.Resource = $"StopPoint?stopTypes=NaptanPublicBusCoachTram&lat={latLong[0]}&lon={latLong[1]}&app_id=7b7f0ddf&app_key=eccb0a1c946585b78547fca93ac5055e";

			var stopResponse = tflClient.Execute<StopApiResponse>(stopsRequest);
			var stopData = stopResponse.Data;
			var nearestStops = stopData.stopPoints;
			List<Stop> sortedNearestStops = nearestStops.OrderBy(o => o.distance).ToList();
			return sortedNearestStops;
		}

		static void PrintBusTimes(string busStopCode)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			var tflClient = new RestClient();
			tflClient.BaseUrl = new Uri("https://api.tfl.gov.uk/");

			var busesRequest = new RestRequest();
			busesRequest.Resource = $"StopPoint/{busStopCode}/Arrivals?app_id=7b7f0ddf&app_key=eccb0a1c946585b78547fca93ac5055e";

			var busData = tflClient.Execute<List<Bus>>(busesRequest).Data;
			List<Bus> SortedBusData = busData.OrderBy(o => o.expectedArrival).ToList();

			var mappedDestinations = SortedBusData.Select(d => d.destinationName).Take(5);
			var mappedLines = SortedBusData.Select(d => d.lineName).Take(5);
			var mappedArrivals = SortedBusData.Select(d => d.expectedArrival).Take(5);

			for (int i = 0; i < 5; i++)
			{
				string formatArrival = String.Format("{0:H mm ss}", mappedArrivals.ElementAt(i));
				Console.WriteLine(mappedLines.ElementAt(i) + "    " + mappedDestinations.ElementAt(i) + "       " + formatArrival);
			}
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Enter Postcode:");
			string postcode = Console.ReadLine();

			List<string> latlong = GetLatLong(postcode);

			List<Stop> busStops = GetBusStopCodes(latlong);

			var nearestStop = busStops[0];
			Console.WriteLine(nearestStop.commonName);
			var nearestStopCode = nearestStop.naptanId;
			PrintBusTimes(nearestStopCode);

			var secondStop = busStops[1];
			Console.WriteLine(secondStop.commonName);
			var secondStopCode = secondStop.naptanId;
			PrintBusTimes(secondStopCode);

			Console.ReadLine();
		}
	}

	public class Bus
	{
		public string lineName { get; set; }
		public string destinationName { get; set; }
		public DateTime expectedArrival { get; set; }
	}

	public class PostcodeApiResponse
	{
		public Postcode result { get; set; }
	}

	public class Postcode
	{
		public string latitude { get; set; }
		public string longitude { get; set; }
	}

	public class StopApiResponse
	{
		public List<Stop> stopPoints { get; set; }
	}


	public class Stop
	{
		public string naptanId { get; set; }
		public float distance { get; set; }
		public string commonName { get; set; }
	}
}
