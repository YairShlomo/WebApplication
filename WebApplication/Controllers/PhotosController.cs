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

        public ActionResult ViewPhoto(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
            //return RedirectToAction("ViewPhoto");
        }
        public ActionResult ResumeDeletion(string fullUrl)
        {
            photosModel.DeletePhoto(fullUrl);
            return RedirectToAction("Photos");
        }
        public ActionResult ConfirmImage(string toDeletePhoto)
        {
            PhotoToDelete = toDeletePhoto;
            Photo photo = new Photo(toDeletePhoto);
            return View(photo);
            //return RedirectToAction("Confirm");
        }
    }
}