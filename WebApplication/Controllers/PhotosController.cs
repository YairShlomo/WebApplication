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
        static string PhotoToDelete;
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
            return View(photosModel);
        }

        public ActionResult ViewPhotos(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
        }
        public ActionResult ResumeDeletion(string fullUrl)
        {
            photosModel.DeletePhoto(fullUrl);
            return RedirectToAction("Photos");
        }
        public ActionResult PhotoDeleteion(string toDeletePhoto)
        {
            PhotoToDelete = toDeletePhoto;
            return RedirectToAction("Confirm");
        }
    }
}