using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfCode2016.Connectivity
{
    public class Properties
    {
        public string name { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties2
    {
        public string name { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public List<double> bbox { get; set; }
        public Geometry geometry { get; set; }
        public Properties2 properties { get; set; }
    }

    public class CityParkJson
    {
        public string type { get; set; }
        public Crs crs { get; set; }
        public List<Feature> features { get; set; }
    }
}