
using System;
using ImageService.Infrastructure.Modal.Event;

namespace ImageServer.WebApplication

{
    public delegate void ExecuteReceivedMessage(CommandRecievedEventArgs arrivedMessage);

    public interface IWebClient
    {
        /// <summary>
        /// Occurs when [execute received].
        /// </summary>
        event ExecuteReceivedMessage ExecuteReceived;
        /// <summary>
        /// Sends the specified command recieved event arguments.
        /// </summary>
        /// <param name="commandRecievedEventArgs">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        void Send(CommandRecievedEventArgs commandRecievedEventArgs);
        /// <summary>
        /// Recieves this instance.
        /// </summary>
        void Recieve();

        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IGuiClient"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        bool Connected { get; set; }
    }

}
