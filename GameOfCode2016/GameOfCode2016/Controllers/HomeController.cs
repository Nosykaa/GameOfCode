using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameOfCode2016.Models;
using GameOfCode2016.Connectivity;
using Newtonsoft.Json;
using System.Globalization;
using GameOfCode2016.CarParkFinder;
using GameOfCode2016.Models.Factory;

namespace GameOfCode2016.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetParks(string address, string expectedHour)
        {
            // TODO check that the string is valid (not an attack)

            double parsedHouer = expectedHour=="" ? 0.0 : double.Parse(expectedHour);

            //CityPark API + finder
            GeoCoderClient myGeoCoderClient = new GeoCoderClient();
            CityParksClient myCityParksClient = new CityParksClient();
            Finder parkFinder = new Finder(myGeoCoderClient, myCityParksClient);

            ResultParkEntity[] resultParks = parkFinder.GetParksByAddress(address);

            //Forecast Factory
            ParkOccupancyFactory forecastFactory = new ParkOccupancyFactory(new Models.DAL.GameOfCode2016Entities());
            foreach (ResultParkEntity parkResult in resultParks)
            {
                if (parkResult.Id != 0)//To avoid Parks without Id !
                {
                    OccupancyForecast occupancyForecast = forecastFactory.GetForecast(parkResult.Id, parsedHouer == 0 ? DateTime.Now : DateTime.Now.AddHours(parsedHouer));
                    if (occupancyForecast != null)
                    {
                        parkResult.FreeSlots = occupancyForecast.FreeSlots;
                        parkResult.Rate = occupancyForecast.Rate;
                    }
                }
            }
            
            //Order result by Rate
            return Json(BuildTop3(resultParks), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPOIFromAddress(string address)
        {
            GeoCoderClient geoClient = new GeoCoderClient();
            GeoLocation geoLocation = geoClient.GetLocationFromAddress(address);

            //GeoCoderClient myGeoCoderClient = new GeoCoderClient();
            //CityParksClient myCityParksClient = new CityParksClient();
            //Finder myFinder = new Finder(myGeoCoderClient, myCityParksClient);

            //ResultParkEntity[] resultParks = myFinder.GetParksByAddress("560 rue de Neudorf 2220 Luxembourg");


            string geoJSon = JsonConvert.SerializeObject(geoLocation.results[0].geomlonlat);
            //string result = @"{""type"": ""FeatureCollection"", ""features"": [";
            return Json(geoLocation.results[0].geomlonlat, JsonRequestBehavior.AllowGet);
            //result += makeJsonItem(geoLocation.results[0]);
            //result += "]}";
            //return Content(result, "application/vnd.geo+json");
        }

        private string makeJsonItem(Result result)
        {
            return @"{""geometry"": {""type"": ""Point"", ""coordinates"": ["+result.geomlonlat.coordinates[0].ToString(CultureInfo.InvariantCulture)+", " + result.geomlonlat.coordinates[1].ToString(CultureInfo.InvariantCulture) + @"]}, ""type"": ""Feature"", ""properties"": {""status"": ""OPEN"", ""contract_name"": ""Luxembourg"", ""name"": """", ""bonus"": false, ""bike_stands"": 15, ""number"": 33, ""last_update"": 1460210716000, ""available_bike_stands"": 14, ""banking"": true, ""available_bikes"": 1, ""address"": """"}}";
        }

        private IEnumerable<ResultParkEntity> BuildTop3(ResultParkEntity[] resultParks)
        {
            int i = 1;
            foreach(ResultParkEntity park in resultParks.Where(f => f.FreeSlots > 30).OrderBy(f => f.Rank).Take(3))
            {
                #region to set-up cost/distance ==> Easly retrieveable from the API
                switch (i)
                {
                    case 1:
                        {
                            park.Cost = "1.5€/H";
                            park.DistanceParkToMyAdress = "1 mins";
                            break;
                        }
                    case 2:
                        {
                            park.Cost = "2€/H";
                            park.DistanceParkToMyAdress = "3 mins";
                            break;
                        }
                    case 3:
                        {
                            park.Cost = "3€/H";
                            park.DistanceParkToMyAdress = "5 mins";
                            break;
                        }

                }
                #endregion
                yield return park;
                i++;
            }
        }
    }
}