using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaiduMapSdk.Entities;
using BaiduMapSdk.Common;
using LBSYunNetSDK.Options;
using BaiduMapSdk.Direction;

namespace BaiduMapApiDemo
{
    class CustomPoiInfo : LbsPoiBaseInfo
    {
        public string BusStation;
        public long BusType;
        public string OnboardTime;
        public Photo StationPhoto;

    }

    class Program
    {
        static void Main(string[] args)
        {
            var ak = "AxXlQ1BehjgOnV5GflqAjrs46iawMsUE";

            #region create table

            var table = new LbsGeotable()
            {
                Name = "MSBus",
                Columns = new List<LbsGeotableColumn>()
                {
                    new LbsGeotableColumn() {Name = "BusStation", Key="BusStation", Type=(int)ColumnType.IsString, MaxLength=20},
                    new LbsGeotableColumn() {Name = "BusType", Key="BusType", Type=(int)ColumnType.IsInt64, MaxLength = 20},
                    new LbsGeotableColumn() { Name = "OnboardTime", Key="OnboardTime", Type= (int)ColumnType.IsString, MaxLength = 20 },
                    new LbsGeotableColumn() {Name = "StationPhoto", Key= "StationPhoto", Type=(int)ColumnType.IsPicUrl }
                }
            };

            table.CreateGeotable(ak);

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
            var origin = new Location { Longitude = 40.056878, Latitude = 116.30815 };
            var destination = new Location { Longitude = 39.915285, Latitude = 116.403857 };
            var time = WebApiDirection.GetDirectionTime(origin, destination, ak);
            Console.WriteLine("From 百度大厦 to 天安门 needs time: {0} s", time);
            #endregion

            #region Get Info
            var poiInfo = table.GetPoiInfo<LbsGeotableBaseResponse<CustomPoiInfo>>(UInt32.Parse(recordIds[0]));
            var stationInfo = poiInfo.contents.FirstOrDefault();
            Console.WriteLine("Get poiInfo BusStation: {0}, BusPic: {1}", stationInfo.BusStation, stationInfo.StationPhoto.big);
            #endregion
        }
    }
}
