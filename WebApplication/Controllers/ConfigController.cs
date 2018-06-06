using ImageServer.WebApplication.Models;
using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServer.WebApplication.Controllers
{
    public class ConfigController :Controller
    {
        private Debug_program debug;
        private static string handlerToDelete;

        static ConfigModel config = new ConfigModel();

        public ConfigController()
        {
            debug = new Debug_program();
            debug.write("ConfigController\n");
            config.Notify -= Notify;
            config.Notify += Notify;
        }


        public void Notify()
        {
            Config();
            // RedirectToAction("Config");
        }
        // GET: Config
        public ActionResult Config()
        {
            return View(config);
        }

        // GET: ResumeDeletion
        public ActionResult ResumeDeletion()
        {
            config.CloseHandler(handlerToDelete);
            return RedirectToAction("Config");
        }
        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(config);
        }


        // GET: Config/DeleteHandler/
        public ActionResult DeleteHandler(string toDeleteHandler)
        {
            
            handlerToDelete = toDeleteHandler;
            return RedirectToAction("Confirm");

        }
    }
}