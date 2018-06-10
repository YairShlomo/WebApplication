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
        static HomeModel home = new HomeModel();
        public HomeController()
        {
            home.Notify -= Notify;
            home.Notify += Notify;
        }
        public void Notify()
        {
            Home();
        }
        public ActionResult Home()
        {
            return View(home);
        }
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
        public ActionResult Photos()
        {
            //ViewBag.Message = "Your Config page.";
            return View(new PhotosModel());
        }
    }
}