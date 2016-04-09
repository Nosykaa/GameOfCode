using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfCode2016.CarParkFinder;
using GameOfCode2016.Models;
using System.Linq;
using GameOfCode2016.Connectivity;

namespace GameOfCode2016.Test.CarParkFinder
{
    [TestClass]
    public class FinderTest
    {
        [TestMethod]
        public void GetParksByAddress_SimpleTest()
        {
            GeoCoderClient myGeoCoderClient = new GeoCoderClient();
            CityParksClient myCityParksClient = new CityParksClient();
            Finder myFinder = new Finder(myGeoCoderClient, myCityParksClient);

            ResultParkEntity[] resultParks = myFinder.GetParksByAddress("560 rue de Neudorf 2220 Luxembourg");

            Assert.AreEqual(26, resultParks.Count());
            Assert.AreEqual("Auchan", resultParks[0].Name);
            Assert.AreEqual("Luxexpo", resultParks[1].Name);
            Assert.AreEqual("Kirchberg", resultParks[2].Name);
        }
    }
}
