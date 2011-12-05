// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using SmallTuba.Network.Object;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class keeps resending packets for the server if its unresponsive
    /// </summary>
    public class ClientFE
    {
        /// <summary>
        /// The name of the client
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The lower level upd communication
        /// </summary>
        private readonly UDPMulticast udpMulticast;

        /// <summary>
        /// A request ID that is increased by one each time a unique packet is send
        /// </summary>
        private long queryId;

        /// <summary>
        /// May I have a new CLIENT_COM with this name?
        /// </summary>
        /// <param name="name">The name of the client</param>
        public ClientFE(string name)
        {
            Contract.Requires(name != null);
            this.name = name;
            this.udpMulticast = new UDPMulticast(1);
        }

        /// <summary>
        /// What is the result of the this query?
        /// The method will keep waiting/requesting an answer till the answer is received or the timeout is reached
        /// </summary>
        /// <param name="message">The query</param>
        /// <param name="timeOut">The time to wait in milliseconds, 0 means forever</param>
        /// <returns>The result, null in the case an answer isn't received</returns>
        public Message SendQuery(Message message, long timeOut)
        {
            Contract.Requires(message != null);
            // Send a query
            this.queryId++;
            Packet packet = new Packet("server", this.name, this.queryId.ToString(), message);
            this.udpMulticast.Send(packet);
            
            // Start listening for a reply
            long preTime = DateTime.Now.ToFileTime();
            while ((DateTime.Now.ToFileTime() < preTime + (timeOut * 10000)) || (timeOut == 0))
            {
                // Wait for reply
                object result = this.udpMulticast.Receive(200);
                if (result == null)
                {
                    // No reply - resend...
                    Console.Out.WriteLine("Client repeats");
                    this.udpMulticast.Send(packet);
                }
                else if (result.GetType().Equals(typeof(Packet)))
                {
                    // A packet was received
                    Packet recPacket = (Packet)result;
                    
                    // Test if the packet is for us
                    if (recPacket.GetReceiverId.Equals(this.name) && recPacket.GetRequestId.Equals(this.queryId.ToString()))
                    {
                        return recPacket.GetMessage;
                    }
                }
            }

            // The timeout has expired
            return null;
        }
    }
}
