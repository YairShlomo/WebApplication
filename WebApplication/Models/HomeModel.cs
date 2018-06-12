using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Modal.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //private string[] extensionsToListen = { "*.jpg", "*.gif", "*.png", "*.bmp" };   // List for valid extensions.
        Object mutexWrite;

        public HomeModel()
        {

            client = WebClient.Instance;
            client.ExecuteReceived += ExecuteReceived;
            client.Recieve();
            mutexWrite = new Object();
            //BindingOperations.EnableCollectionSynchronization(Handlers, mutexWrite);
            string[] Args = new string[5];
            CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, Args, "");
            lock (mutexWrite)
            {
                client.Send(request);

            }

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
                    /*
                    if (Int32.Parse(arrivedMessage.Args[0]) == (int)CommandEnum.NewFileCommand)
                    {
                         countOutput++;
                    }
                   */
                   if (arrivedMessage.CommandID == (int)CommandEnum.NewFileCommand)
                    {
                        countOutput++;

                    }

                    else if (arrivedMessage.CommandID == (int)CommandEnum.LogCommand)
                    {

                        foreach (Log log in JsonConvert.DeserializeObject<ObservableCollection<Log>>(arrivedMessage.Args[0]))
                        {
                            if (log.Message.Contains("Add File"))
                            {
                                countOutput++;
                            }
                        }

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