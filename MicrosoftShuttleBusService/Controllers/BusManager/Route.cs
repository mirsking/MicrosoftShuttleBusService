using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusManager
{
    public class Route
    {
        public List<Station> Stations;

        public Route()
        {
            Stations = new List<Station>();
        }
        public Route(List<Station> stationlist)
        {
            Stations = new List<Station>(stationlist);
        }

        public void AddStationToRoute(Station s)
        {
            this.Stations.Add(s);
        }

        public Station FindStation(Station s)
        {
            foreach (Station station in Stations)
            {
                if (station.GetName() == s.GetName())
                    return station;
            }
            return null;
        }

    }
}
