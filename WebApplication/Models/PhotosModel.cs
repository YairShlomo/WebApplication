using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ImageServer.WebApplication.Models
{
    public class PhotosModel
    {
        public delegate void NotifyAboutChange();
        public event NotifyAboutChange Notify;
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        private WebClient client { get; set; }
        private ObservableCollection<string> RegularPhotos { get; set; }
        private ObservableCollection<string> ThumbnailPhotos { get; set; }

        public PhotosModel()
        {
            client = WebClient.Instance;
            //client.Recieve();
            client.ExecuteReceived += ExecuteReceived;
            RegularPhotos = new ObservableCollection<string>();
            ThumbnailPhotos = new ObservableCollection<string>();

            //InitData();

        }

        /// <summary>
        /// Executes the received.
        /// </summary>
        /// <param name="arrivedMessage">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void ExecuteReceived(CommandRecievedEventArgs arrivedMessage)
        {
            //  debug.write("ExecuteReceived\n");
            try
            {
                if (arrivedMessage != null)
                {
                    switch (arrivedMessage.CommandID)
                    {
                        case (int)CommandEnum.NewFileCommand:
                            AddPhoto(arrivedMessage.Args[0]);
                            break;
                        case (int)CommandEnum.DeleteFileCommand:
                            DeletePhoto(arrivedMessage.Args[0]);

                            break;
                        default:
                            //client.Close();
                            break;
                    }

                    //Notify?.Invoke();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }
        public void AddPhoto(string path)
        {
            //split to regular photo-place[0]. and thumbnail photo-place [1]
            string[] splitted=  path.Split(';');

            //add only thumbnail photo -which should be viewed
            RegularPhotos.Add(splitted[0]);
            ThumbnailPhotos.Add(splitted[1]);

        }
        public void DeletePhoto(string path)
        {
            //get index of thubmnail.
            int index =ThumbnailPhotos.IndexOf(path);
            ThumbnailPhotos.RemoveAt(index);
            RegularPhotos.RemoveAt(index);

        }
        public void SendDeletePhoto(string path)
        {
            Object mutexWrite = new Object();
            string[] Args = { path };
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.DeleteFileCommand, Args, "");
            Console.WriteLine((int)commandRecievedEventArgs.CommandID + "\n");
            lock (mutexWrite)
            {
                client.Send(commandRecievedEventArgs);

            }
        }

    }
}