using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace BusManager
{
    public class Station : Point
    {
        public int Route;

        public bool IsRoute;

        public string Name;

        public List<int> ToHomeTime;

        public string ToHomePic;

        public List<int> ToCompTime;

        public string ToCompPic;

        public Station()
        {
            Route = 0;
            IsRoute = true;
            LocX = 0;
            LocY = 0;
            Name = "";
            ToHomeTime = new List<int>();
            ToHomePic = "";
            ToCompTime = new List<int>();
            ToCompPic = "";
        }

        public Station(int r, bool isr, double x, double y, string name, List<int> tht, string thp, List<int> tct, string tcp) : base(x,y)
        {
            Route = r;
            IsRoute = isr;
            Name = name;
            ToHomeTime = new List<int>(tht.ToArray());
            ToHomePic = thp;
            ToCompTime = new List<int>(tct.ToArray());
            ToCompPic = tcp;
        }

        public Station(Station s)
        {
            Route = s.Route;
            IsRoute = s.IsRoute;
            LocX = s.LocX;
            LocY = s.LocY;
            Name = s.Name;
            ToHomeTime = new List<int>(s.ToHomeTime);
            ToHomePic = s.ToHomePic;
            ToCompTime = new List<int>(s.ToCompTime);
            ToCompPic = s.ToCompPic;
        }

        public List<int> GetToCompTime()
        {
            return this.ToCompTime;
        }

        public string GetName()
        {
            return this.Name;
        }


        /*static void Main(string[] args)
        {
            //BusUtil.FindNearestStations(new Point(121.398921, 31.106097), 5);
            //List<Station> allStations = DataAccess.ReadAllStations();
            //Console.WriteLine(DataAccess.AllStationsToJson());
            var allStations = DataAccess.ReadAllStations();
            var allRoutes = DataAccess.ReadAllRoutes();
            Station MicroSoft = allStations[4];
            Console.WriteLine(BusUtil.BestStationsToJson(new Point(121.398921, 31.106097), MicroSoft));
            Console.ReadKey();
        }*/

    }
}
