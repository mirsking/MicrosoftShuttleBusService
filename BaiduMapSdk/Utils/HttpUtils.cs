using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaiduMapSdk.Utils
{
    public static class HttpUtils
    {
        public static HttpWebResponse GetResponse(string method, Uri uri, Dictionary<string, string> keyValues)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            if (String.Compare(method, "GET") == 0)
            {
                string uriParam = "?";
                foreach (var kv in keyValues)
                {
                    var name = kv.Key.ToLower();
                    var value = kv.Value;
                    uriParam = uriParam + "&" + name + "=" + value;
                }
                request = (HttpWebRequest)WebRequest.Create(uri + uriParam);
                request.ContentType = "application/x-www-form-urlencoded";
            }
            else if (String.Compare(method, "POST", true) == 0)
            {
                throw new NotImplementedException();
            }


            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }
    }
}
