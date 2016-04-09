using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfCode2016.Connectivity;
using GameOfCode2016.Models;
using System.Linq;
using System.Collections.Generic;

namespace GameOfCode2016.Test
{
    [TestClass]
    public class GeoCoderClientTest
    {
        [TestMethod]
        public void GetLocationFromAddress_SimpleTest()
        {
            GeoCoderClient myGeoCoderClient = new GeoCoderClient();

            string address = "560 rue de Neudorf 2220 Luxembourg";

            GeoLocation resultGeoLocation = myGeoCoderClient.GetLocationFromAddress(address);

            Assert.AreEqual(true, resultGeoLocation.success);
            Assert.AreEqual(1, resultGeoLocation.results.Count());
            Assert.AreEqual(8, resultGeoLocation.results[0].accuracy);
            Assert.AreEqual("560 Rue de Neudorf,2220 Luxembourg", resultGeoLocation.results[0].address);
            Assert.AreEqual(6.18447006860937, resultGeoLocation.results[0].geomlonlat.coordinates[0]);
            Assert.AreEqual(49.6223621694221, resultGeoLocation.results[0].geomlonlat.coordinates[1]);
            //Assert.AreEqual(new List<double>() {6.18447006860937, 49.6223621694221 }, resultGeoLocation.results[0].GetLonLatCoordinates());
        }

        [TestMethod]
        public void GetLocationFromAddress_ResultFail()
        {
            GeoCoderClient myGeoCoderClient = new GeoCoderClient();

            string address = "Chez moi";

            GeoLocation resultGeoLocation = myGeoCoderClient.GetLocationFromAddress(address);

            Assert.AreEqual(true, resultGeoLocation.success);
            Assert.AreEqual(5, resultGeoLocation.results[0].accuracy);
        }
    }
}
