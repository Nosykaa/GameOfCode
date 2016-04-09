using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using GameOfCode2016.Models;
using Newtonsoft.Json;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace GameOfCode2016.Connectivity
{
    /// <summary>
    /// see https://data.public.lu/en/datasets/mobilite-circulation/
    /// </summary>
    public class CityParksClient
    {
        //private string ParkAndBikeUrl = "http://opendata.vdl.lu/odaweb/index.jsp?cat=4ea917f2155670ea92f17af0";
        private string ParkAndRideUrl = "http://opendata.vdl.lu/odaweb/index.jsp?cat=4ea917f2155670ea92f17af1";
        private string ParkingCouvertUrl = "http://opendata.vdl.lu/odaweb/index.jsp?cat=4ea917f2155670ea92f17bf4";
        private string ParkingMobiliteReduite = "http://opendata.vdl.lu/odaweb/index.jsp?cat=4ea917f2155670ea92f17bf2";
        private string ParkingSurface = "http://opendata.vdl.lu/odaweb/index.jsp?cat=4ea917f2155670ea92f17bf3";

        private string vdlUrl = "http://service.vdl.lu/rss/circulation_guidageparking.php";

        // Do not use JSON Infos redodant with vdl infos
        public CityCarPark[] AggregateAllParksAvailable()
        {
            //string jsonstringParkAndRide = GET(ParkAndRideUrl);
            //CityParkJson parkAndRideObject = JsonConvert.DeserializeObject<CityParkJson>(jsonstringParkAndRide);
            string jsonstringParkingCouvert = GET(ParkingCouvertUrl);
            CityParkJson parkingCouvertObject = JsonConvert.DeserializeObject<CityParkJson>(jsonstringParkingCouvert);
            //string jsonstringParkingMobiliteReduite = GET(ParkAndRideUrl);
            //CityParkJson ParkingMobiliteReduiteObject = JsonConvert.DeserializeObject<CityParkJson>(jsonstringParkingMobiliteReduite);
            string jsonstringParkingSurface = GET(ParkAndRideUrl);
            CityParkJson ParkingSurfaceObject = JsonConvert.DeserializeObject<CityParkJson>(jsonstringParkingSurface);


            List<Feature> jsonCarParks = 
                parkingCouvertObject.features
                //.Union(ParkingMobiliteReduiteObject.features)
                .Union(ParkingSurfaceObject.features).ToList(); 
            CityCarPark[] cityParks = GetCityParks();
            CityCarPark[] resultParks = cityParks.Union(jsonCarParks.Select(k => new CityCarPark { 
                Title = k.properties.name,  
                LocalisationLongitude = k.geometry.coordinates[0], 
                LocalisationLatitude = k.geometry.coordinates[1]})).ToArray();
            string test = resultParks.Select(k => k.Title).OrderBy(k => k).Aggregate((cur, nex) => cur + "\n" + nex);
            return resultParks;
        }

        // Returns JSON string
        private string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        /// <summary>
        /// see http://stackoverflow.com/questions/6294948/pull-rss-feeds-from-facebook-page
        /// </summary>
        /// <returns></returns>
        public CityCarPark[] GetCityParks()
        {
            List<CityCarPark> parks = new List<CityCarPark>();

            var req = (HttpWebRequest)WebRequest.Create(vdlUrl);
            req.Method = "GET";
            req.UserAgent = "Fiddler";
            var rep = req.GetResponse();
            var reader = XmlReader.Create(rep.GetResponseStream());
            SyndicationFeed feed = SyndicationFeed.Load(reader);


            foreach (SyndicationItem park in feed.Items)
            {
                parks.Add(new CityCarPark()
                {
                    Id =  (int?)GetElement<int>(park, "id"),
                    Title = park.Title.Text,
                    QuartierFR = (string)GetElement<string>(park, "quartier"),
                    //QuartierDE = GetElement<Int32>(park, "quartier"),
                    Total = (int?)GetElement<int>(park, "total"),
                    Actuel = (int?)GetElement<int>(park, "actuel"),
                    //Tendance = (int?)GetElement<int>(park, "tendance"),
                    Ouvert = (int?)GetElement<int>(park, "ouvert"),
                    Complet = (int?)GetElement<int>(park, "complet"),
                    //PictureUrl = (string)GetElement<string>(park, "divers", "vdlxml:pictureUrl"),

                    LocalisationLatitude = (double?)GetElement<double?>(park, "localisation", "vdlxml:localisationLatitude"),
                    LocalisationLongitude = (double?)GetElement<double?>(park, "localisation", "vdlxml:localisationLongitude"),
                    
                    //LocalisationEntree = park.ElementExtensions.Where(f => f.OuterName == "localisationEntree").DefaultIfEmpty(null).FirstOrDefault().GetObject<string>(),
                    //LocalisationSortie = park.ElementExtensions.Where(f => f.OuterName == "localisationSortie").DefaultIfEmpty(null).FirstOrDefault().GetObject<string>(),
                    //LocalisationLatitude = park.ElementExtensions.Where(f => f.OuterName == "localisationLatitude").DefaultIfEmpty(null).FirstOrDefault().GetObject<double>(),
                    //LocalisationLongitude = park.ElementExtensions.Where(f => f.OuterName == "localisationLongitude").DefaultIfEmpty(null).FirstOrDefault().GetObject<double>(),
                    //NominalSurface = park.ElementExtensions.Where(f => f.OuterName == "nominalSurface").DefaultIfEmpty(null).FirstOrDefault().GetObject<int>(),
                    //NominalCouvertes = park.ElementExtensions.Where(f => f.OuterName == "nominalCouvertes").DefaultIfEmpty(null).FirstOrDefault().GetObject<int>(),
                    //NominalHandicapes = park.ElementExtensions.Where(f => f.OuterName == "nominalHandicapes").DefaultIfEmpty(null).FirstOrDefault().GetObject<int>(),
                    //NominalFemmes = park.ElementExtensions.Where(f => f.OuterName == "nominalFemmes").DefaultIfEmpty(null).FirstOrDefault().GetObject<int>(),
                });
            }

            return parks.ToArray();
        }


        private object GetElement<T>(SyndicationItem item, string name, string subName = null)
        {
            var element = item.ElementExtensions.Where(f => f.OuterName == name).DefaultIfEmpty(null).FirstOrDefault();

            if (element != null)
            {
                if (subName != null)
                {
                    var reader = element.GetReader();
                    reader.ReadToDescendant(subName);
                    var t = reader.ReadElementContentAsDouble();
                    if (t != null)
                        return t;
                    else
                        return null;
                }
                else
                {
                    string res = element.GetObject<string>();
                    if (res != "")
                        return element.GetObject<T>();
                    else
                        return null;
                }
            }
            else
                return null;
        }
    }
}