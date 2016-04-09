using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfCode2016.Helpers;
using System.Collections.Generic;

namespace GameOfCode2016.Test.Connectivity
{
    [TestClass]
    public class DistanceHelperTest
    {
        [TestMethod]
        public void GetDistanceBetweenCoordinates_SimpleTest()
        {
            double distanceCalculated = DistanceHelper.GetDistanceBetweenCoordinates(new List<double>() { 0.00D, 0.00D }, new List<double>() { 0.00D, 0.00D });

            Assert.AreEqual(0.00D, distanceCalculated);
        }

        [TestMethod]
        public void GetDistanceBetweenCoordinates_Compare_From_Two_Parks()
        {
            // 0.0 0.0 is our destination address
            // 1.1 1.1 is our first park
            // 1.2 1.1 is our second park
            double distanceCalculatedForFirstPark = DistanceHelper.GetDistanceBetweenCoordinates(new List<double>() { 0.00D, 0.00D }, new List<double>() { 1.1D, 1.1D });
            double distanceCalculatedForSecondPark = DistanceHelper.GetDistanceBetweenCoordinates(new List<double>() { 0.00D, 0.00D }, new List<double>() { 1.2D, 1.1D });

            Assert.AreEqual(true, distanceCalculatedForSecondPark > distanceCalculatedForFirstPark);
        }
    }
}
