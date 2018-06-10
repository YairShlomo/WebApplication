using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
namespace ImageServer.WebApplication.Models
{
    public class HomeModel
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
        public WebClient client { get; set; }
        private uint countOutput = 0;
        private int count = 0;
        static string fileName = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/info.txt");
        private string[] studentsInfo = System.IO.File.ReadAllLines(fileName);
        

        public HomeModel()
        {

            client = WebClient.Instance;
            client.ExecuteReceived += ExecuteReceived;
        }
        
        

        public string[] students()
        {
            return studentsInfo;
        }
        private void ExecuteReceived(CommandRecievedEventArgs arrivedMessage)
        {
            try
            {
                if (arrivedMessage != null)
                {
                    if (Int32.Parse(arrivedMessage.Args[0]) == (int)CommandEnum.NewFileCommand)
                    {
                        if (count < 2)
                            count++;
                        else
                            countOutput++;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public uint getOutputCount()
        {
            return this.countOutput;
        }
        public string check()
        {
             return fileName;
            //return "";
        }
    }
}