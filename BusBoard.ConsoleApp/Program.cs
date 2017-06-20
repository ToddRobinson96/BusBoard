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
		static void PrintBusTimes(List<Bus> sortedBusData)
		{
			foreach (Bus b in sortedBusData.Take(5))
			{
				string formatArrival = String.Format("{0:H mm ss}", b.expectedArrival);
				Console.WriteLine(b.lineName + "    " + b.destinationName + "       " + formatArrival);
			}
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Enter Postcode:");
			string postcode = Console.ReadLine();
			var PostcodeApiClient = new PostcodeApiClient();
			Postcode postcodeData = PostcodeApiClient.GetLatLong(postcode);

			var TflApiClient = new TflApiClient();
			List<Stop> busStops = TflApiClient.GetBusStopCodes(postcodeData);

			int[] zeroOne = new int[] { 0, 1 };
			foreach (int i in zeroOne)
			{
				var nextStop = busStops[i];
				Console.WriteLine(nextStop.commonName);
				var nearestStopCode = nextStop.naptanId;
				PrintBusTimes(TflApiClient.GetBusTimes(nearestStopCode));
			}

			Console.ReadLine();
		}
	}
}
