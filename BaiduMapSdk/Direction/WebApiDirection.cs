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
    public enum DirectionMode
    {
        driving,
        walking,
        transit,
        riding
    }


    public static class WebApiDirection
    {
        public static uint GetDirectionTime(Location origin, Location destination, string ak, DirectionMode mode = DirectionMode.transit)
        {
            var jObject = GetDirection(origin, destination, ak, mode);
            if (jObject["status"].ToString() == "0")
            {
                var routes = jObject["result"]["routes"];
                if (routes != null)
                {
                    string duration = "0";
                    switch (mode)
                    {
                        case DirectionMode.transit:
                            duration = routes[0]["scheme"][0]["duration"].ToString();
                            break;
                        case DirectionMode.walking:
                            duration = routes[0]["duration"].ToString();
                            break;
                        default:
                            throw new NotSupportedException("Right now not support type: " + mode.ToString());
                    }

                    return UInt32.Parse(duration);
                }
                else
                {
                    // too close and no routes
                    return GetDirectionTime(origin, destination, ak, DirectionMode.walking);
                }
            }
            else
            {
                throw new NotSupportedException(JsonConvert.SerializeObject(jObject));
            }
        }

        public static JObject GetDirection(Location origin, Location destination, string ak, DirectionMode mode = DirectionMode.transit)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("ak",ak);
            values.Add("origin", origin.Latitude.ToString() + "," + origin.Longitude.ToString());
            values.Add("destination", destination.Latitude.ToString() + "," + destination.Longitude.ToString());
            values.Add("mode", mode.ToString());
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
