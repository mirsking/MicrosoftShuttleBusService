using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusManager
{
    class DataAccess
    {
        static string HomeDirectory = @"C:\Users\t-qiche\Documents\Visual Studio 2015\Projects\MicrosoftShuttleBusService\MicrosoftShuttleBusService\";
        static string path = HomeDirectory + "Controllers\\BusManager\\shuttle.txt";
        static string StationFile = HomeDirectory + "Controllers\\BusManager\\AllStations";
        static string RouteFile = HomeDirectory +　"Controllers\\BusManager\\AllRoutes";

        public static List<Station> Read()          //Initialize and generate the data
        {
            var AllStations = new List<Station>();
            var AllRoutes = new List<Route>();
            
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine())!=null)
            {
                
                var trimmedline = line.TrimEnd('\t');
                AllStations.Add(CreateFromString(trimmedline));
            }
            WriteAllStations(AllStations);

            AllRoutes = GenerateRoutes(AllStations);
            WriteAllRoutes(AllRoutes);

            return AllStations;
        }

        public static void WriteAllStations(List<Station> AllStations)
        {
            string json = JsonConvert.SerializeObject(AllStations);
            StreamWriter sw = new StreamWriter(StationFile, false);
            sw.Write(json);
            sw.Close();
        }

        public static void WriteAllRoutes(List<Route> AllRoutes)
        {
            string json = JsonConvert.SerializeObject(AllRoutes);
            StreamWriter sw = new StreamWriter(RouteFile, false);
            sw.Write(json);
            sw.Close();
        }

        public static List<Route> GenerateRoutes(List<Station> allStations)
        {
            var allRoutes = new List<Route>();
            int size = allStations.Count;
            int flag = -1;
            for (int i = 0; i < size; ++i)
            {
                if (allStations[i].Route != flag)
                {
                    flag = allStations[i].Route;
                    var sl = new List<Station>();
                    allRoutes.Add(new Route(sl));
                }
                Station tmpStation = new Station(allStations[i]);
                allRoutes[allRoutes.Count-1].AddStationToRoute(tmpStation);
            }
            return allRoutes;
        }

        private static Station CreateFromString(string line)
        {
            string[] para = line.Split('\t');
            bool isr = para[0]=="Route"?true:false;
            int r = Convert.ToInt32(para[1]);
            double x = Convert.ToDouble(para[2].Split(',')[0]);
            double y = Convert.ToDouble(para[2].Split(',')[1]);
            string name = para[3];
            List<int> ToHomeTime = new List<int>(),ToCompTime = new List<int>();
            string ToHomePic="",ToCompPic="";

            int i = 4, size = para.Length;
            while (i < size)
            {
                if (para[i].EndsWith(".jpg"))
                {
                    break;
                }
                if (para[i] != "---")
                {
                    ToCompTime.Add(Convert.ToInt32(para[i].Split('：')[0]) * 60 + Convert.ToInt32(para[i].Split('：')[1]));
                }
                else
                {
                    ToCompTime.Add(-1);
                }
                i++;
            }
            ToCompPic = para[i];
            i++;
            while (i < size)
            {
                if (para[i] != "---")
                {
                    ToHomeTime.Add(Convert.ToInt32(para[i].Split('：')[0]) * 60 + Convert.ToInt32(para[i].Split('：')[1]));
                }
                i++;
            }
            return new Station(r,isr,x,y,name,ToHomeTime,ToHomePic,ToCompTime,ToCompPic);
        }

        public static List<Station> ReadAllStations()
        {
            StreamReader sr = new StreamReader(StationFile);
            //string xml = sr.ReadToEnd();
            string json = sr.ReadToEnd();
            //List<Station> allStations = XmlUtil.Deserialize(typeof(List<Station>), xml) as List<Station>;
            List<Station> allStations = JsonConvert.DeserializeObject<List<Station>>(json);
            return allStations;
        }

        public static List<Route> ReadAllRoutes()
        {
            StreamReader sr = new StreamReader(RouteFile);
            //string xml = sr.ReadToEnd();
            string json = sr.ReadToEnd();
            //List<Route> allRoutes = XmlUtil.Deserialize(typeof(List<Route>), xml) as List<Route>;
            List<Route> allRoutes = JsonConvert.DeserializeObject<List<Route>>(json);
            return allRoutes;
        }

        
    }
}