using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace BusBoard.Api
{
	public class TflApiClient
	{
		RestClient tflClient = new RestClient();
		public TflApiClient()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			tflClient.BaseUrl = new Uri("https://api.tfl.gov.uk/");
		}

		public List<Stop> GetBusStopCodes(Postcode postcodeData)
		{
			var stopsRequest = new RestRequest();
			stopsRequest.Resource = $"StopPoint?stopTypes=NaptanPublicBusCoachTram&lat={postcodeData.latitude}&lon={postcodeData.longitude}&app_id=7b7f0ddf&app_key=eccb0a1c946585b78547fca93ac5055e";

			var stopResponse = tflClient.Execute<StopApiResponse>(stopsRequest);
			var stopData = stopResponse.Data;
			var nearestStops = stopData.stopPoints;
			List<Stop> sortedNearestStops = nearestStops.OrderBy(o => o.distance).ToList();
			return sortedNearestStops;
		}

		public List<Bus> GetBusTimes(string busStopCode)
		{
			var busesRequest = new RestRequest();
			busesRequest.Resource = $"StopPoint/{busStopCode}/Arrivals?app_id=7b7f0ddf&app_key=eccb0a1c946585b78547fca93ac5055e";

			var busData = tflClient.Execute<List<Bus>>(busesRequest).Data;
			List<Bus> sortedBusData = busData.OrderBy(o => o.expectedArrival).ToList();

			return sortedBusData;
		}
	}
}
