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
		static void Main(string[] args)
		{
			Console.WriteLine("Enter Bus Stop Code");
			string busStopCode = Console.ReadLine();

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			var client = new RestClient();
			client.BaseUrl = new Uri("https://api.tfl.gov.uk/");

			var request = new RestRequest();
			request.Resource = "StopPoint/"+ busStopCode + "/Arrivals?app_id=7b7f0ddf&app_key=eccb0a1c946585b78547fca93ac5055e";

			var data = client.Execute<List<bus>>(request).Data;

			var mappedDestinations = data.Select(d => d.destinationName).Take(5);
			var mappedLines = data.Select(d => d.lineName).Take(5);
			var mappedArrivals = data.Select(d => d.expectedArrival).Take(5);

			for (int i = 0; i < 5; i++)
			{
				string formatArrival = String.Format("{0:H mm ss}", mappedArrivals.ElementAt(i));
				Console.WriteLine(mappedLines.ElementAt(i) + "    " + mappedDestinations.ElementAt(i) + "       " + formatArrival);
			}

			//Console.WriteLine("   {0}",String.Concat(mappedData));
			Console.ReadLine();
		}
	}

	public class bus
	{
		public string lineName { get; set; }
		public string destinationName { get; set; }
		public DateTime expectedArrival { get; set; }
	}
}
