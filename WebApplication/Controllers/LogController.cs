using ImageServer.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServer.WebApplication.Controllers
{
    public class LogController : Controller
    {
        static LogModel logModel = new LogModel();

        public LogController()
        {

            logModel.Notify -= Notify;
            logModel.Notify += Notify;
        }
        public void Notify()
        {
            Logs();
        }
        // GET: Log
        public ActionResult Logs()
        {
            return View(logModel);
        }
    }
}