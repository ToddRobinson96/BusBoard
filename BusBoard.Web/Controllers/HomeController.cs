using System.Web.Mvc;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using BusBoard.Api;

namespace BusBoard.Web.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public ActionResult BusInfo(PostcodeSelection selection)
    {
			bool valid = true;
			var PostcodeApiClient = new PostcodeApiClient();
			Postcode postcodeData = PostcodeApiClient.GetLatLong(selection.Postcode);

			if (postcodeData == null)
			{
				valid = false;
			}

			var TflApiClient = new TflApiClient();
			List<Stop> stops = TflApiClient.GetBusStopCodes(postcodeData);

			if (!stops.Any())
			{
				valid = false;
			}

			List<List<Bus>> buses = new List<List<Bus>>();
			foreach (Stop s in stops)
			{
				buses.Add(TflApiClient.GetBusTimes(s.naptanId));
			}

			var info = new BusInfo(selection.Postcode)
			{
				PostCode = selection.Postcode,
				Stops = stops,
				Buses = buses,
				Valid = valid
			};
      return View(info);
    }

    public ActionResult About()
    {
      ViewBag.Message = "Information about this site";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Contact us!";

      return View();
    }
  }
}