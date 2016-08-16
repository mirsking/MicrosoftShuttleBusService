﻿using System;
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

        public class BaiduRoute
        {
            public int status;
            public string message;
            public int type;

        }
        public static int TimeBetweenStations(Station start, Station end)
        {
            int RouteNumber = 39;
            int startRoute = (start.IsRoute ? 0 : 1)*RouteNumber + start.Route;
            int endRoute = (end.IsRoute ? 0 : 1) * RouteNumber + end.Route;
            if (startRoute != endRoute)
                return -1;
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
            return -1;
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
            List<List<Point>> jsonstring = new List<List<Point>>();
            for (int i = 0; i < size; ++i)
            {
                List<Point> route = new List<Point>();
                Route r = allRoutes[NumberToRealNumber(bestStations[i].Route, bestStations[i].IsRoute)];
                var routestations = r.Stations;
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
                        route.Add(new Point(routestations[j].LocX, routestations[j].LocY));
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
                route.Add(new Point(y.LocX, y.LocY));
                jsonstring.Add(route);
            }
            return JsonConvert.SerializeObject(jsonstring);
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

            /*StreamWriter sw = new StreamWriter("C:\\t-zel\\younghackathon\\BusManager\\BusManager\\try");
            for (int i = 0; i < size; ++i)
            {
                Console.WriteLine(list[i].Key);
                sw.WriteLine(allStations[list[i].Value].Name);
            }
            sw.Close();*/
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