using ImageServer.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServer.WebApplication.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logs()
        {
           // ViewBag.Message = "Your application description page.";

            return View(new LogModel());
        }

        public ActionResult Config()
        {
            //ViewBag.Message = "Your Config page.";
            return View(new ConfigModel());
        }
    }
}