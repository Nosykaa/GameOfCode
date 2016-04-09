using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfCode2016.Models
{
    public class Geomlonlat
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Geom
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class AddressDetails
    {
        public string zip { get; set; }
        public string locality { get; set; }
        public string id_caclr_street { get; set; }
        public string street { get; set; }
        public string postnumber { get; set; }
        public string id_caclr_building { get; set; }
    }

    public class Result
    {
        public double ratio { get; set; }
        public string name { get; set; }
        public double easting { get; set; }
        public string address { get; set; }
        public Geomlonlat geomlonlat { get; set; }
        public Geom geom { get; set; }
        public double northing { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public string __invalid_name__matching_street { get; set; }
        public int accuracy { get; set; }

        //public List<double> GetLonLatCoordinates()
        //{
        //    return this.geomlonlat.coordinates;
        //}
    }

    public class GeoLocation
    {
        public int count { get; set; }
        public List<Result> results { get; set; }
        public bool success { get; set; }
    }
}
    