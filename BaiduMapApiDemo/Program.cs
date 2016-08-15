using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using BaiduMapSdk.Entities;
using BaiduMapSdk.Common;
using LBSYunNetSDK.Options;
using BaiduMapSdk.Direction;
using System.Web.Script.Serialization;
using System.Collections;

namespace BaiduMapApiDemo
{
    class CustomPoiInfo : LbsPoiBaseInfo
    {
        public string BusStation;
        public long BusType;
        public string OnboardTime;
        public Photo StationPhoto;

    }

    class SitePoiInfo : LbsPoiBaseInfo
    {
        public string alias;
        public string phone;
        public string position;

    }

    public class BaiduMapApi
    {
        #region  Table Defination
        static string ak = "AxXlQ1BehjgOnV5GflqAjrs46iawMsUE";
        static IList<LbsGeotableColumn> columns = new List<LbsGeotableColumn>()
        {
            new LbsGeotableColumn() { Name = "Alias", Key="Alias", Type=(int)ColumnType.IsString, MaxLength=20},
            new LbsGeotableColumn() { Name = "Phone", Key="Phone", Type=(int)ColumnType.IsInt64, MaxLength = 20},
            new LbsGeotableColumn() { Name = "Position", Key="Position", Type= (int)ColumnType.IsString, MaxLength = 20 },
        };
        #endregion

        private static LbsGeotable tableNewSite = new LbsGeotable(ak, "MBBus_NewSite", columns);

        public static string addSite(string alias, string phone, string position, string lng, string lat)
        {
            var record = new Dictionary<string, string>();
            record.Add("Position", position);
            record.Add("Alias", alias);
            record.Add("Phone", phone);   
            var id = tableNewSite.AddOneRecord(Double.Parse(lng), Double.Parse(lat), record);
            return id;
        }

        public static String getSites()
        {
            var poiInfo = tableNewSite.GetAllPoiInfo<SitePoiInfo>();

            ArrayList eventList = new ArrayList();
            foreach (var item in poiInfo.contents)
            {
                Hashtable ht = new Hashtable();
                ht.Add("lng", (item.location.First()));
                ht.Add("lat", item.location.Last());
                ht.Add("Alias", item.alias);
                ht.Add("Phone", item.phone);
                ht.Add("Position", item.position);
                eventList.Add(ht);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String jsonStr = ser.Serialize(eventList);
            return jsonStr;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            #region create table

            string ak = "AxXlQ1BehjgOnV5GflqAjrs46iawMsUE";
            var columns = new List<LbsGeotableColumn>()
            {
                new LbsGeotableColumn() {Name = "BusStation", Key="BusStation", Type=(int)ColumnType.IsString, MaxLength=20},
                new LbsGeotableColumn() {Name = "BusType", Key="BusType", Type=(int)ColumnType.IsInt64, MaxLength = 20},
                new LbsGeotableColumn() { Name = "OnboardTime", Key="OnboardTime", Type= (int)ColumnType.IsString, MaxLength = 20 },
                new LbsGeotableColumn() {Name = "StationPhoto", Key= "StationPhoto", Type=(int)ColumnType.IsPicUrl }
            };

            var table = new LbsGeotable(ak, "MSBus", columns);

            #endregion


            #region add record
            var recordIds = new List<string>();
            string[,] data = new string[2,5]{ { "121.591907", "31.286229", "东陆路", "7:20", "http://img2.3lian.com/2014/f6/173/d/51.jpg" },
                { "121.59371", "31.276292", "张杨北路,五莲路",    "7:25",    "http://img2.3lian.com/2014/f6/173/d/51.jpg"} };

            for (int i = 0; i < data.Rank; i++)
            {
                var record = new Dictionary<string, string>();
                record.Add("BusStation", data[i, 2]);
                record.Add("OnboardTime", data[i, 3]);
                record.Add("StationPhoto", data[i, 4]);
                var id = table.AddOneRecord(Double.Parse(data[i, 0]), Double.Parse(data[i, 1]), record);
                recordIds.Add(id);
            }

            #endregion


            #region Direction
            //parsed json
            var origin = new Location { Longitude = 40.056878, Latitude = 116.30815 };
            var destination = new Location { Longitude = 39.915285, Latitude = 116.403857 };
            var time = WebApiDirection.GetDirectionTime(origin, destination, table.Ak);
            Console.WriteLine("From 百度大厦 to 天安门 needs time: {0} s", time);

            //return json directly, you can see the json format from here http://lbsyun.baidu.com/index.php?title=webapi/direction-api#.E5.85.AC.E4.BA.A4.E8.B7.AF.E5.BE.84.E8.A7.84.E5.88.92.E8.BF.94.E5.9B.9E.E5.80.BC.E8.AF.B4.E6.98.8E
            var json = WebApiDirection.GetDirection(origin, destination, table.Ak);
            Console.Write("Json is \n" + json);
            #endregion

            #region Get Info
            var poiInfoCollection = table.GetAllPoiInfo<CustomPoiInfo>();
            var onePoiInfo = poiInfoCollection.contents.FirstOrDefault();
            #endregion
        }
    }
}
