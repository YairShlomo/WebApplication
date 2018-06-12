using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public ObservableCollection<string> RegularPhotos { get; set; }
        public ObservableCollection<string> ThumbnailPhotos { get; set; }
        public List<Photo> PhotoList { get; set; }

        public PhotosModel()
        {
            client = WebClient.Instance;
            //client.Recieve();
            client.ExecuteReceived += ExecuteReceived;
            RegularPhotos = new ObservableCollection<string>();
            ThumbnailPhotos = new ObservableCollection<string>();
            PhotoList = new List<Photo>();
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
                    if (arrivedMessage.CommandID == (int)CommandEnum.NewFileCommand)
                    {
                        AddPhoto(arrivedMessage.Args[1]);
                    }
                    else if (arrivedMessage.CommandID == (int)CommandEnum.DeleteFileCommand)
                    {
                        DeletePhoto(arrivedMessage.Args[0]);
                    }


                    Notify?.Invoke();
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
            string[] splitted = path.Split(';');

            //add only thumbnail photo -which should be viewed
            //RegularPhotos.Add(splitted[1]);
           // ThumbnailPhotos.Add(splitted[0]);
            Photo photo = new Photo(splitted[1]);
            PhotoList.Add(photo);
        }
        public void DeletePhoto(string path)
        {
            Photo photo1 = null;
            //get index of thubmnail.
            // int index =ThumbnailPhotos.IndexOf(path);
            // string pathThumb = ThumbnailPhotos[index];
            // RegularPhotos.RemoveAt(index);
            // ThumbnailPhotos.RemoveAt(index);
             foreach (Photo photo in PhotoList)
             {
                 if (string.Compare(photo.ImageFullUrl,path)==0)
                 {
                    photo1 = photo;
                 }
             }
            PhotoList.Remove(photo1);
            //PhotoList.Clear();
            Notify?.Invoke();

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
      /** [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePathThumbnail")]
        public string ImageRelativePathThumbnail { get; set; }**/

    }
}