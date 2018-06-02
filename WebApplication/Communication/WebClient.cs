
using System;
using ImageService.Infrastructure.Modal;
using ImageService.Infrastructure.Modal.Event;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;

namespace ImageServer.WebApplication
{
    public class WebClient
    {

        private static WebClient instance;
        private TcpClient client;
        public bool Connected { get; set; }
       // private bool isStopped;
        private static Mutex mutex = new Mutex();
        public event ExecuteReceivedMessage ExecuteReceived;
        BinaryReader reader;
        BinaryWriter writer;

        private WebClient()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                Connected = true;
                NetworkStream stream = client.GetStream();
                writer = new BinaryWriter(stream);
                reader = new BinaryReader(stream);
                Console.WriteLine("You are connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Connected = false;
                Console.WriteLine("You are not connected");

            }
        }
        public static WebClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebClient();
                }
                return instance;
            }
        }
        public void Send(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            new Task(() =>
            {
                try
                {
                    if (Connected)
                    {
                        Console.WriteLine($"Sendbeforejson {commandRecievedEventArgs.RequestDirPath} to Server");
                        string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                        // Send data to server
                        Console.WriteLine($"Send {jsonCommand} to Server");
                        // debug.write("send from Guiclient" + jsonCommand + "\n");
                        mutex.WaitOne();
                        writer.Write(jsonCommand);
                        mutex.ReleaseMutex();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"excption thrown sender" + e.Message);
                    Close();

                }
            }).Start();
        }
        public void Recieve()
        {
            new Task(() =>
            {
                try
                {
                    Console.WriteLine("Recived"+ Connected);
                    while (Connected)
                    {
                      
                        string jsonArrivedMessage = reader.ReadString();
                      // Debug.WriteLine("recived client:" + jsonArrivedMessage + "\n");
                       Console.WriteLine($"Recieve {jsonArrivedMessage} from Server");
                        CommandRecievedEventArgs arrivedMessage = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(jsonArrivedMessage);
                        
                        ExecuteReceived?.Invoke(arrivedMessage);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"excption thrown reciver"+e.Message);
                    string[] Args = { "2", $"excption thrown reciver" + e.Message };
                    CommandRecievedEventArgs cre = new CommandRecievedEventArgs((int)CommandEnum.AddLog, Args, null);
                    ExecuteReceived?.Invoke(cre);
                    Close();


                }
            }).Start();
        }
        public void Close()
        {
            Connected = false;
            /*
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseClient, null, "");
            Send(commandRecievedEventArgs);
            client.Close();
            Connected = false;
            */
        }
    }
}


