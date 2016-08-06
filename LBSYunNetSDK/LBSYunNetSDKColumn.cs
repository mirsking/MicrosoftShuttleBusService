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

namespace LBSYunNetSDK
{
    public partial class LBSYunNet
    {
        #region geo Column

        #region Post

        public LBSYunNetSDKResult ColumnCreate(string columnName, string columnKey, UInt32 columnType, UInt32 maxLength,
            string defaultValue,
            UInt32 isSortfilterField, UInt32 isSearchField, UInt32 isIndexField, UInt32 isUniqueField,
            string geotableId)
        {
            //??????????????????
            //百度地图LBS云存储APIv3.0接口说明文档.doc
            //geotable_id	所属于的geotable_id	String(50)	 必选  Page 17
            string paraUrlCoded = "name=" + columnName + "&key=" + columnKey + "&type=" + columnType + "&max_length=" + maxLength
                + "&default_value=" + defaultValue
                + "&is_sortfilter_field=" + isSortfilterField + "&is_search_field=" + isSearchField
                + "&is_index_field=" + isIndexField
                + "&is_unique_field=" + isUniqueField + "&geotable_id=" + geotableId
                + "&ak=" + _ak;
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.column.ToString(),
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

        public LBSYunNetSDKResult ColumnUpdate(UInt32 columnId, string columnKey, UInt32 columnType, UInt32 maxLength,
            UInt32 isSortfilterField, UInt32 isSearchField, UInt32 isIndexField, UInt32 isUniqueField,
            UInt32 geotableId, string defaultValue = null, string columnName = null)
        {
            string paraUrlCoded = "id=" + columnId + "&key=" + columnKey + "&type=" + columnType + "&max_length=" + maxLength
                + "&is_sortfilter_field=" + isSortfilterField + "&is_search_field=" + isSearchField
                + "&is_index_field=" + isIndexField
                + "&is_unique_field=" + isUniqueField + "&geotable_id=" + geotableId
                + "&ak=" + _ak;

            if (!String.IsNullOrEmpty(columnName))
            {
                paraUrlCoded += ("&name=" + columnName);
            }
            if (!String.IsNullOrEmpty(defaultValue))
            {
                paraUrlCoded += ("&default_value=" + defaultValue);
            }
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.column.ToString(),
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

        public LBSYunNetSDKResult ColumnDelete(UInt32 columnId, UInt32 geotableId)
        {
            string paraUrlCoded = "id=" + columnId + "&geotable_id=" + geotableId + "&ak=" + _ak;

            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.POST.ToString(),
                entity: LBSYunNetSDKEntitys.column.ToString(),
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

        #region GET

        public LBSYunNetSDKResult ColumnList(string geotableId, string key = null, string columnName = null)
        {
            //??????????????????
            //百度地图LBS云存储APIv3.0接口说明文档.doc
            //geotable_id	所属于的geotable_id	String(50)	必选  Page 19
            string paraUrlCoded = "ak=" + _ak + "&geotable_id=" + geotableId;
            if (!string.IsNullOrEmpty(key))
            {
                paraUrlCoded += ("&key=" + key);
            }
            if (!string.IsNullOrEmpty(columnName))
            {
                paraUrlCoded += ("&name=" + columnName);
            }
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            string getData = "?" + paraUrlCoded;
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.GET.ToString(),
                entity: LBSYunNetSDKEntitys.column.ToString(),
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

        public LBSYunNetSDKResult ColumnDetail(UInt32 geotableId, UInt32 columnId)
        {
            string paraUrlCoded = "ak=" + _ak + "&geotable_id=" + geotableId + "&id=" + columnId;
            if (!String.IsNullOrEmpty(_sn))
            {
                paraUrlCoded += ("&sn=" + _sn);
            }
            string getData = "?" + paraUrlCoded;
            HttpWebResponse response = GetResponse(
                method: LBSYunNetSDKMethods.GET.ToString(),
                entity: LBSYunNetSDKEntitys.column.ToString(),
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
    }
}
