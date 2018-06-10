using ImageServer.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServer.WebApplication.Controllers
{
    public class PhotosController : Controller
    {
        static PhotosModel photosModel = new PhotosModel();
        // GET: Photos
        public PhotosController()
        {
            photosModel.Notify -= Notify;
            photosModel.Notify += Notify;
        }
        public void Notify()
        {
            Photos();
        }
        public ActionResult Photos()
        {
            return View();
        }
    }
}