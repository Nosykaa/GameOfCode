using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfCode2016.Connectivity;
using GameOfCode2016.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfCode2016.Test.Connectivity
{
    [TestClass]
    public class CityParksClientTest
    {
        [TestMethod]
        public void GetCityParks_SimpleTest()
        {
            CityParksClient client = new CityParksClient();

            CityCarPark[] parks = client.GetCityParks();
        }

        //[TestMethod]
        //public void AggregateAllParksAvailable_SimpleTest()
        //{
        //    CityParksClient client = new CityParksClient();
        //    CityCarPark[] cityParkList = client.AggregateAllParksAvailable();

        //    Assert.AreEqual(32, cityParkList.Count());
        //}
    }
}
