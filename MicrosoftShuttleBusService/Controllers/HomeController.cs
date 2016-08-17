using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaiduMapApiDemo;
using BusManager;
using BaiduMapSdk.Entities;

namespace MicrosoftShuttleBusService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BaiduMap()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public String AddSite(FormCollection collection)
        {
            string alias = collection["aliasInput"];
            string phone = collection["phoneInput"];
            string address = collection["positionInput"];
            string lng = collection["lngInput"];
            string lat = collection["latInput"];

            if (string.IsNullOrEmpty(phone))
            {
                return BaiduMapApi.getSites();
            }
            return BaiduMapApi.addSite(alias, phone, address, lng, lat);


        }

        public string AllStationsInJson()
        {
            return BusUtil.AllStationsToJson();
        }


        public string BestStationsInJson(FormCollection collection)
        {
            double x = 0;
            double y = 0;
            Point l = new Point(x, y);
            var allStations = DataAccess.ReadAllStations();
            Station MicroSoft = allStations[4];
            return BusUtil.BestStationsToJson(l, MicroSoft);
        }
		
		public void EditStation(FormCollection collection)
        {
            string name = "上海站";
			string route = "Route1";
			double x = 0;
			double y = 0;
            BusUtil.AdminEditStation(name,route,x,y);
        }
    }
}