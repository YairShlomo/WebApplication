using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

using ImageService.Infrastructure.Enums;
//using System.Windows.Data;
using ImageService.Infrastructure.Modal;
using ImageService.Infrastructure.Modal.Event;
using ImageService.Communication;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ImageService;

namespace ImageServer.WebApplication.Models
{
    public class ConfigModel
    {
        private string m_tumbnailSize;
        private string m_outputDirectory;
        private string m_sourceName;
        private string m_logName;
        //private Debug_program dp;

        //private ObservableCollection<string> mHandlers;
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void NotifyAboutChange();

        public event NotifyAboutChange Notify;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingModel"/> class.
        /// </summary>
        public ConfigModel()
        {
            client = WebClient.Instance;
            client.Recieve();
            client.ExecuteReceived += ExecuteReceived;
            InitData();
            
        }
        #region Notify Changed

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
        public WebClient client { get; set; }
        /// <summary>
        /// Initializes the data.
        /// </summary>
        private void InitData()
        {
            try
            {
                OutputDirectory = string.Empty;
                SourceName = string.Empty;
                LogName = string.Empty;
                TumbnailSize = string.Empty;
                Handlers = new ObservableCollection<string>();
                //dp = new Debug_program();

                //m_Handlers.Insert(0,"dfdsfs");
                Object thisLock = new Object();
                //BindingOperations.EnableCollectionSynchronization(Handlers, thisLock);
                string[] Args = new string[5];
                CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, Args, "");
                client.Send(request);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Executes the received.
        /// </summary>
        /// <param name="arrivedMessage">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void ExecuteReceived(CommandRecievedEventArgs arrivedMessage)
        {
            try
            {
                if (arrivedMessage != null)
                {
                    //dp.write("get to ExecuteReceived" + arrivedMessage.Args[0] +"\n");

                    switch (arrivedMessage.CommandID)
                    {
                        case (int)CommandEnum.GetConfigCommand:
                            Update(arrivedMessage);
                            break;
                        case (int)CommandEnum.CloseHandlerCommand:
                            CloseHandler(arrivedMessage);
                            break;
                        default:
                            //client.Close();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

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
                //dp.write("yey\n");

                TcpMessages tcpMessages = JsonConvert.DeserializeObject<TcpMessages>(arrivedMessage.Args[0]);
                OutputDirectory = tcpMessages.Args[0];
                SourceName = tcpMessages.Args[1];
                LogName = tcpMessages.Args[2];
                TumbnailSize = tcpMessages.Args[3];
                for (int i = 4; i < tcpMessages.Args.Length; i++)
                {
                    Handlers.Add(tcpMessages.Args[i]);
                }
                int k = 0;
                foreach(string item in Handlers)
                {
                   // dp.write(k+":::::"+item+"\n");

                }
                Notify?.Invoke();



            }
            catch (Exception e)
            {
                Console.WriteLine("exception in update-settingmodel" + e.Message);

            }
        }
        /// <summary>
        /// Closes the handler.
        /// </summary>
        /// <param name="arrivedMessage">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void CloseHandler(CommandRecievedEventArgs arrivedMessage)
        {
            if (Handlers != null && Handlers.Count > 0 && arrivedMessage.Args != null
                                 && Handlers.Contains(arrivedMessage.Args[0]))
            {
                Handlers.Remove(arrivedMessage.Args[0]);
            }
        }

        /// <summary>
        /// Closes the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void CloseHandler(string handler)
        {
            Console.WriteLine(handler);
            string[] Args = { handler };
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseHandlerCommand, Args, handler);
            client.Send(commandRecievedEventArgs);
        }
        //members
        [Required]
        [DataType(DataType.Text)]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "Thumbnail Size")]
        public string TumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; private set; }
    }

}

    
