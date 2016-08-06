using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

using LBSYunNetSDK.Results;
using LBSYunNetSDK.Options;
using LBSYunNetSDK.Utils;

namespace LBSYunNetSDK
{
    /// <summary>
    /// Baidu LBSYun SDK
    /// API: lbsyun.baidu.com
    /// </summary>
    public partial class LBSYunNet
    {
        private const string API_DOMAIN = "api.map.baidu.com/geodata/v3";
        private const string DL = "/";
        private readonly string _ak;
        private readonly string _sn;
        public const string ApiVersion = "3.0";
        public const string Version = "1.0.0";

        public LBSYunNet(string ak, string sn = null)
        {
            this._ak = ak;
            this._sn = sn;
        }

        #region geotable

        #region Post

        public LBSYunNetSDKResult GeotableCreate(string geotableName, int geoType, int isPublished, UInt32 timestamp)
        {
            string paraUrlCoded = "name=" + geotableName + "&geotype=" + geoType + "&is_published=" + isPublished + "&timestamp=" + timestamp + "&ak=" + _ak;
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.geotable.ToString(),
                operation: LBSYunNetSDKOperations.create.ToString(),
                postData: postData
                );

            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string json = sr.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            LBSYunNetSDKResult re = jss.Deserialize<LBSYunNetSDKResult>(json);
            return re;
        }
        public LBSYunNetSDKResult GeotableUpdate(int geotableId, int isPublished, string geoTableName = null)
        {
            string paraUrlCoded = "id=" + geotableId + "&is_published=" + isPublished + "&ak=" + _ak;
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            if (!String.IsNullOrEmpty(geoTableName))
            {
                paraUrlCoded += ("&name=" + geoTableName);
            }
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.geotable.ToString(),
                operation: LBSYunNetSDKOperations.update.ToString(),
                postData: postData
                );
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string json = sr.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            LBSYunNetSDKResult re = jss.Deserialize<LBSYunNetSDKResult>(json);
            return re;
        }
        public LBSYunNetSDKResult GeotableDelete(int geotableId)
        {
            string paraUrlCoded = "id=" + geotableId + "&ak=" + _ak;
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.geotable.ToString(),
                operation: LBSYunNetSDKOperations.delete.ToString(),
                postData: postData
                );
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string json = sr.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            LBSYunNetSDKResult re = jss.Deserialize<LBSYunNetSDKResult>(json);
            return re;
        }
        #endregion
        #region Get
        public LBSYunNetSDKResult GeotableList(string geotableName)
        {
            string paraUrlCoded = "ak=" + _ak;
            if (!string.IsNullOrEmpty(geotableName))
            {
                paraUrlCoded += ("&name=" + geotableName);
            }

            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            string getData = "?" + paraUrlCoded;
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.GET.ToString(),
                entity: LBSYunNetSDKEntitys.geotable.ToString(),
                operation: LBSYunNetSDKOperations.list.ToString(),
                getData: getData
                );
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string json = sr.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            LBSYunNetSDKResult re = jss.Deserialize<LBSYunNetSDKResult>(json);
            return re;
        }
        public LBSYunNetSDKResult GeotableDetail(int geotableId)
        {
            string paraUrlCoded = "ak=" + _ak + "&id=" + geotableId.ToString();

            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            string getData = "?" + paraUrlCoded;
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.GET.ToString(),
                entity: LBSYunNetSDKEntitys.geotable.ToString(),
                operation: LBSYunNetSDKOperations.detail.ToString(),
                getData: getData
                );
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string json = sr.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            LBSYunNetSDKResult re = jss.Deserialize<LBSYunNetSDKResult>(json);
            return re;
        }
        #endregion
        #endregion
        #region GetResponse
        private HttpWebResponse GetResponse(string method, string entity, string operation, byte[] postData = null, string getData = null, Hashtable headers = null)
        {
            if (getData != null)
            {
                operation += getData;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + API_DOMAIN + DL + entity + DL + operation);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            if (headers != null)
            {
                foreach (DictionaryEntry var in headers)
                {
                    request.Headers.Add(var.Key.ToString(), var.Value.ToString());
                }
            }

            if (method==LBSYunNetSDKMethods.POST.ToString())
            {
                if (postData != null)
                {
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(postData, 0, postData.Length);
                    dataStream.Close();
                }
            }
            else if(method==LBSYunNetSDKMethods.GET.ToString()){

            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //this.tmp_infos = new Hashtable();
                //foreach (var hl in response.Headers)
                //{
                //    string name = (string)hl;
                //    if (name.Length > 7 && name.Substring(0, 7) == "x-upyun")
                //    {
                //        this.tmp_infos.Add(name, response.Headers[name]);
                //    }
                //}
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        private HttpWebResponse GetResponse2(string method, string entity, string operation, Dictionary<string,string> postData = null, string getData = null, Dictionary<string,string> headers = null)
        {
            if (getData != null)
            {
                operation += getData;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + API_DOMAIN + DL + entity + DL + operation);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    request.Headers.Add(h.Key.ToString(), h.Value.ToString());
                }
            }

            if (method==LBSYunNetSDKMethods.POST.ToString())
            {
                if (postData != null)
                {
                    var boundary = "----" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                    request.ServicePoint.Expect100Continue = false;
                    Stream dataStream = request.GetRequestStream();
                    postData.WriteMultipartFormData(dataStream, boundary);
                    byte[] endBytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");
                    dataStream.Write(endBytes, 0, endBytes.Length);
                    dataStream.Close();
                }
            }
            else if(method==LBSYunNetSDKMethods.GET.ToString()){

            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //this.tmp_infos = new Hashtable();
                //foreach (var hl in response.Headers)
                //{
                //    string name = (string)hl;
                //    if (name.Length > 7 && name.Substring(0, 7) == "x-upyun")
                //    {
                //        this.tmp_infos.Add(name, response.Headers[name]);
                //    }
                //}
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Get Unix TimeSpan
        /// </summary>
        /// <param name="time"> </param>
        /// <returns></returns>
        public static UInt32 GetUnixTimeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (UInt32)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// Get DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(UInt32 unixTimeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(unixTimeStamp);
            return dtStart.Add(toNow);
        }
        #endregion

    }
}
