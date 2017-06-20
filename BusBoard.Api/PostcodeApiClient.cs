using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace BusBoard.ConsoleApp
{
	public class PostcodeApiClient
	{
		RestClient postcodeClient = new RestClient();
			
		public PostcodeApiClient()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			postcodeClient.BaseUrl = new Uri("http://api.postcodes.io/");
		}

		public Postcode GetLatLong(string postcode)
		{
			var postcodeRequest = new RestRequest();
			postcodeRequest.Resource = $"postcodes/{postcode}";

			var apiResponse = postcodeClient.Execute<PostcodeApiResponse>(postcodeRequest).Data;
			var postcodeData = apiResponse.result;

			return postcodeData;
		}
	}
}
