using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BaiduMapSdk;
using BaiduMapSdk.Direction;
using BaiduMapSdk.Entities;

namespace BusManager
{
    static class BusUtil
    {
        public class jsonStation
        {
            public string name;
            public double LocX;
            public double LocY;
            public jsonStation(string n, double x, double y)
            {
                name = n;
                LocX = x;
                LocY = y;
            }
        }

        public class jsonRoute
        {
            public string RouteNumber;
            public List<jsonStation> Stations;
            public jsonRoute(string n, List<jsonStation> stations)
            {
                RouteNumber = n;
                Stations = new List<jsonStation>(stations);
            }
        }

        public static int TimeBetweenStations(Station start, Station end)
        {
            int RouteNumber = 39;
            int startRoute = (start.IsRoute ? 0 : 1)*RouteNumber + start.Route;
            int endRoute = (end.IsRoute ? 0 : 1) * RouteNumber + end.Route;
            if (startRoute != endRoute)
                return 100000;
            return GetTimeWithSameSchedule(start.GetToCompTime(), end.GetToCompTime());
        }

        private static int GetTimeWithSameSchedule(List<int> start, List<int> end)
        {
            int size = start.Count;
            for (int i = 0; i < size; ++i)
            {
                if (start[i] < 0 || end[i] < 0)
                {
                    continue;
                }
                return Math.Abs(start[i] - end[i]);
            }
            return 100000;
        }
        public static List<Station> FindBestStation(Point x, Station y)
        {
            int MAX = 100000;
            int NUM = 3;
            int RouteNumber = 39;
            int k = 5;
            var allRoutes = DataAccess.ReadAllRoutes();
            var nearest = FindNearestStations(x,k);
            var Selected = new List<KeyValuePair<Station, double>>();
            for (int i = 0; i < k; ++i)
            {
                double time = FindRouteByApi(x,nearest[i]);
                Route r = allRoutes[NumberToRealNumber(nearest[i].Route, nearest[i].IsRoute)];
                Station s = r.FindStation(y);
                time += TimeBetweenStations(s, nearest[i]);
                Selected.Add(new KeyValuePair<Station, double>(nearest[i],time));
            }
            Selected.Sort(cmp_BestStation);
            List<Station> bestStations = new List<Station>();
            for (int i = 0;i<NUM;++i)
            {
                bestStations.Add(Selected[i].Key);
            }
            return bestStations;
        }
        public static int NumberToRealNumber(int route, bool isRoute)
        {
            if (isRoute)
            {
                int[] r = { 0, 0, 1, 1, 1, 2, 3, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 19, 19, 19, 19, 19, 19, 19, 19, 20, 21, 22, 22, 23, 24, 25, 26 };
                return r[route];
            }
            else
            {
                return route + 26;
            }
        }
        public static string BestStationsToJson(Point x, Station y)
        {
            var bestStations = FindBestStation(x, y);
            int size = bestStations.Count;
            var allRoutes = DataAccess.ReadAllRoutes();
            List<jsonRoute> jsonString = new List<jsonRoute>();
            for (int i = 0; i < size; ++i)
            {
                List<jsonStation> jstations = new List<jsonStation>();
                
                Route r = allRoutes[NumberToRealNumber(bestStations[i].Route, bestStations[i].IsRoute)];
                var routestations = r.Stations;
                string routenumber = (routestations[0].IsRoute ? "Route" : "RT") + Convert.ToString(routestations[0].Route);
                int s = routestations.Count;
                int j = 0;
                while(j<s)
                {
                    if (routestations[j].GetName() != bestStations[i].GetName())
                        j++;
                    else
                    {
                        break;
                    }
                }
                while(j<s)
                {
                    if (routestations[j].GetName() != y.GetName())
                    {
                        jstations.Add(new jsonStation(routestations[j].Name,routestations[j].LocX, routestations[j].LocY));
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
                jstations.Add(new jsonStation(y.Name,y.LocX, y.LocY));
                jsonString.Add(new jsonRoute(routenumber,jstations));
            }
            return JsonConvert.SerializeObject(jsonString);
        }

        public static double FindRouteByApi(Point x, Point y)
        {
            string ak = "AxXlQ1BehjgOnV5GflqAjrs46iawMsUE";
            Location start = new Location() { Longitude = x.LocX, Latitude = x.LocY };
            Location end = new Location() { Longitude = y.LocX, Latitude = y.LocY };

            return (double)WebApiDirection.GetDirectionTime(start, end, ak) / 60;
        }

        public static List<Station> FindNearestStations(Point start, int k)
        {
            var allStations = DataAccess.ReadAllStations();
            int size = allStations.Count;
            var list = new List<KeyValuePair<double, int>>();
            for (int i = 0; i < size; ++i)
            {
                list.Add(new KeyValuePair<double, int>(Point.ApproxDistance(start,allStations[i]), i));
            }
            list.Sort(cmp);

            var nearestStations = new List<Station>();
            for (int i = 0; i < k; ++i)
            {
                nearestStations.Add(allStations[list[i].Value]);
            }
            return nearestStations;
        }

        public static string AllStationsToJson()
        {
            List<Station> allStations = DataAccess.ReadAllStations();
            return JsonConvert.SerializeObject(allStations);
        }

        public static string CheckRouteToJson(string route, bool isToComp)
        {
            string r = string.Join("", route.Split());
            bool isRoute = r.StartsWith("Route");
            int routeNo = Convert.ToInt32(isRoute ? r.Substring(5) : r.Substring(2));
            List<Route> allRoutes = DataAccess.ReadAllRoutes();
            Route checkRoute = allRoutes[NumberToRealNumber(routeNo, isRoute)];
            List<Point> stations = new List<Point>();
            if (isToComp)
            {
                for(int i = 0;i<checkRoute.Stations.Count;++i)
                {
                    stations.Add(new Point(checkRoute.Stations[i].LocX, checkRoute.Stations[i].LocY));
                }
            }
            else
            {
                {
                    for (int i = checkRoute.Stations.Count-1; i >= 0; --i)
                    {
                        stations.Add(new Point(checkRoute.Stations[i].LocX, checkRoute.Stations[i].LocY));
                    }
                }
            }
            return JsonConvert.SerializeObject(stations);
        }

        public static Station GetStationFromName(string s)
        {
            var allStations = DataAccess.ReadAllStations();
            foreach (Station station in allStations)
            {
                if (station.Name == s)
                    return station;
            }
            return null;
        }

        public static void AdminEditStation(string name, string route, double newLocationX, double newLocationY)
        {
            var allStations = DataAccess.ReadAllStations();
            foreach (Station s in allStations)
            {
                string rn = (s.IsRoute ? "Route" : "RT") + Convert.ToString(s.Route);
                if (s.Name == name && rn == route)
                {
                    s.LocX = newLocationX;
                    s.LocY = newLocationY;
                    DataAccess.WriteAllStations(allStations);
                    return;
                }
            }
        }
        public static string AllRoutesToJson()
        {
            List<Route> allRoutes = DataAccess.ReadAllRoutes();
            return JsonConvert.SerializeObject(allRoutes);
        }

        public static void AddNewPoint(double x, double y)
        {
            new Point(x, y);
        }

        private static int cmp(KeyValuePair<double, int> a, KeyValuePair<double, int> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private static int cmp_BestStation(KeyValuePair<Station, double> a, KeyValuePair<Station, double> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }

}
