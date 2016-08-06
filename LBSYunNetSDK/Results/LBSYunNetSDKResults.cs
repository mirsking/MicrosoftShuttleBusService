using System;
using System.Collections.Generic;

namespace LBSYunNetSDK.Results
{
    #region Results

    [Serializable]
    public struct LBSYunNetSDKResult
    {
        //success = 0,others...........
        public int status;
        //describe for status
        public string message;
        // new data id
        public string id;
        public UInt32 size;
        public UInt32 total;
        public List<Geotable> geotables;
        public Geotable geotable;
        public List<Column> columns;
        public Column column;
    }

    [Serializable]
    public struct Geotable
    {
        public string id;
        public int Geotype;
        public string Name;
        public int Is_published;
        public string Create_time;
        public string Modify_time;
    }

    [Serializable]
    public struct Column
    {
        public string id;
        public string geotable_id;
        public string name;
        public string key;
        public UInt32 type;
        public UInt32 max_length;
        public string default_value;
        public string create_time;
        public string modify_time;
        public UInt32 is_sortfilter_field;
        public UInt32 is_search_field;
        public UInt32 is_index_field;
        public UInt32 is_unique_field;
    }

    public interface IPoi
    {
    }


    class Location
    {
        public double latitude;
        public double longitude;
    }

    [Serializable]
    class PoiBase: IPoi
    {
        public string title;
        public Location location;
        public string city;
        public string create_time;
        public string geotable_id;
        public string address;
        public string province;
        public string district;
        public string city_id;
        public string id;
    }

    #endregion
}
