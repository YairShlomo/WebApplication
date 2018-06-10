using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServer.WebApplication.Models
{
    public class PhotosModel
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
    }
}