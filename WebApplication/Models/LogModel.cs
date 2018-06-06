using System;
using System.ComponentModel;

using ImageService.Infrastructure.Enums;
//using System.Windows.Data;
using ImageService.Logging;
using ImageService.Infrastructure.Modal;
using ImageService.Infrastructure.Modal.Event;
using System.Windows;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ImageService;

namespace ImageServer.WebApplication.Models
{
    public class LogModel
    {
        private ObservableCollection<Log> m_Log;
        public delegate void NotifyAboutChange();

        public event NotifyAboutChange Notify;
      //  Debug_program debug;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogModel"/> class.
        /// </summary>
        public LogModel()
        {
            client = WebClient.Instance;
            //client.Recieve();
            client.ExecuteReceived += ExecuteReceived;
            InitData();
           // debug = new Debug_program();
        }
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
        /*
        public ObservableCollection<Log> Logs
        {
            get
            {
                return m_Log;
            }
        }
        */
        /**  public void setLogs(Log log)
          {
              m_Log.Add(log);
              OnPropertyChanged("Log");
          }**/

        /// <summary>
        /// Initializes the data.
        /// </summary>
        private void InitData()
        {
            try
            {
               Logs= new ObservableCollection<Log>();
                Object mutexWrite = new Object();
                //BindingOperations.EnableCollectionSynchronization(logs, mutexWrite);
                string[] Args = new string[5];
                CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, Args, "");
                Console.WriteLine((int)commandRecievedEventArgs.CommandID + "\n");
                lock (mutexWrite)
                {
                    client.Send(commandRecievedEventArgs);

                }
                // debug.write("InitData\n");
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
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
                        case (int)CommandEnum.LogCommand:
                            Update(arrivedMessage);
                            break;
                        case (int)CommandEnum.AddLog:
                            AddLog(arrivedMessage);
                            break;
                        default:
                            //client.Close();
                            break;
                    }
                    foreach (Log log in Logs) {
                     // debug.write(log.Message);
                    }
                   
                    Notify?.Invoke();
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the specified arrived message.
        /// </summary>
        /// <param name="arrivedMessage">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void Update(CommandRecievedEventArgs arrivedMessage)
        {
            try
            {
                foreach (Log log in JsonConvert.DeserializeObject<ObservableCollection<Log>>(arrivedMessage.Args[0]))
                {
                    this.Logs.Add(log);
                    // setLogs(log);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("exception in update-logmodel" + e.Message);

                //MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Adds the log.
        /// </summary>
        /// <param name="responseObj">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void AddLog(CommandRecievedEventArgs responseObj)
        {
            try
            {
                //int s = Convert.ToInt32(responseObj.Args[0]);

                //MessageTypeEnum foo = (MessageTypeEnum)Enum.ToObject(typeof(MessageTypeEnum), s);

                Log newLogEntry = new Log { Type = (responseObj.Args[0]), Message = responseObj.Args[1] };
                Logs.Insert(0, newLogEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception in Addlog-logmodel" + e.Message);

               // MessageBox.Show(e.ToString());
            }
        }
        //members

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public ObservableCollection<Log> Logs { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
