using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaiduMapApiDemo;

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

            return BaiduMapApi.addSite(alias, "","","23","23");


        }
    }
}