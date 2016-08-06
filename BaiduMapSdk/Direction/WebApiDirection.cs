using BaiduMapSdk.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaiduMapSdk.Utils;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BaiduMapSdk.Direction
{
    public static class WebApiDirection
    {

        public static uint GetDirectionTime(Location origin, Location destination, string ak)
        {
            var jObject = GetDirection(origin, destination, ak);
            if (jObject["status"].ToString() == "0")
            {
                return UInt32.Parse(jObject["result"]["routes"][0]["scheme"][0]["duration"].ToString());
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static JObject GetDirection(Location origin, Location destination, string ak)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("ak",ak);
            values.Add("origin", origin.Longitude.ToString() + "," + origin.Latitude.ToString());
            values.Add("destination", destination.Longitude.ToString() + "," + destination.Latitude.ToString());
            values.Add("mode", "transit");
            values.Add("region", "上海");
            values.Add("output", "json");
            var uri = new Uri("http://api.map.baidu.com/direction/v1");
            var res = HttpUtils.GetResponse("GET", uri, values);
            var s = res.GetResponseStream();
            var sr = new StreamReader(s);
            string jsonStr = sr.ReadToEnd();
            return JsonConvert.DeserializeObject(jsonStr) as JObject;
        }
    }
}
