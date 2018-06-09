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
        private static LogModel logModel = new LogModel();

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
        [HttpPost]
        public ActionResult LogChangeFilter(FormCollection form)
        {
            string type = form["typeFilter"].ToString();
            logModel.FilterType = type;
            SelectedLogUpdate(type);
            return RedirectToAction("Logs");
        }
        public void SelectedLogUpdate(string type)
        {
            if (type != "")
            {
                logModel.SelectedLogMessages.Clear();
                foreach (Log item in logModel.Logs)
                {
                    if (item.Type == type)
                    {
                        logModel.AddToSelected(item);
                    }
                }
            }
            else
            {
                logModel.SelectedLogMessages.Clear();
            }
        }
    }
}