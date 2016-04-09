using GameOfCode2016.Connectivity;
using GameOfCode2016.Helpers;
using GameOfCode2016.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfCode2016.CarParkFinder
{
    public class Finder
    {
        private GeoCoderClient geoCoder;
        private CityParksClient cityParks;
        public Finder(GeoCoderClient geoCoder, CityParksClient cityParks)
        {
            this.geoCoder = geoCoder;
            this.cityParks = cityParks;
        }

        public ResultParkEntity[] GetParksByAddress(string address)
        {
            // Address to coordinates
            GeoLocation destinationLocation = geoCoder.GetLocationFromAddress(address);
            // list parks
            CityCarPark[] carParks = cityParks.GetCityParks();
            // measure
            CityCarPark[] carParksOrdered = carParks.OrderBy(k => DistanceHelper.GetDistanceBetweenCoordinates(destinationLocation.results[0].geomlonlat.coordinates, 
                                            new List<double>() {k.LocalisationLatitude ?? 0.0D, k.LocalisationLongitude ?? 0.0D})).ToArray();
            int i = 1;
            ResultParkEntity[] resultParkEntities = carParksOrdered.Select(k => new ResultParkEntity() { Id = k.Id ?? 0, Name = k.Title, FreeSlots = k.Actuel ?? 0, Rank = i++ }).ToArray();
            // top 3
            return resultParkEntities;
        }
    }
}