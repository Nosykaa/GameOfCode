using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameOfCode2016.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace GameOfCode2016.Connectivity
{
    /// <summary>
    /// see http://wiki.geoportail.lu/doku.php?id=en:api:rest
    /// </summary>
    public class GeoCoderClient
    {

        public GeoLocation GetLocationFromAddress(string address)
        {

            string jsonstringLocation = GET("http://api.geoportail.lu/geocoder/search?queryString=" + address);
            GeoLocation LocationObject = JsonConvert.DeserializeObject<GeoLocation>(jsonstringLocation);
            return LocationObject;
        }

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
    }
}