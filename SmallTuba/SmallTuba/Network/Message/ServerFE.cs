// -----------------------------------------------------------------------
// <copyright file="ServerCom.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using SmallTuba.Network.Object;
    using System.Text;

    /// <summary>
    /// A class that listens for requests and if a request is repeated the respsonse is repeated
    /// </summary>
    public class ServerFE
    {
        /// <summary>
        /// The name of the client
        /// </summary>
        private string name;

        /// <summary>
        /// Used for sending and receiving multicasts
        /// </summary>
        private UDPMulticast udpMulticast;

        /// <summary>
        /// A key/value pair of previous requests and responses
        /// </summary>
        private Dictionary<string, Packet> prevPackets; 

        /// <summary>
        /// Invoke this function when a call is received
        /// </summary>
        private RequestHandler requestHandler = null;
        
        /// <summary>
        /// May I have a new SERVER_FE with this name?
        /// </summary>
        /// <param name="name">The name of the server</param>
        public ServerFE(string name)
        {
            Contract.Requires(name != null);
            this.name = name;
            this.udpMulticast = new UDPMulticast(0);
            this.prevPackets = new Dictionary<string, Packet>();
        }

        /// <summary>
        /// A type of function to invoke when a message is received
        /// </summary>
        /// <param name="query">The query from the client</param>
        /// <returns>The result to the client</returns>
        public delegate Message RequestHandler(Message query);

        /// <summary>
        /// Invoke this function when a call is received
        /// </summary>
        /// <param name="handler">The function to invoke</param>
        public void SetRequestHandler(RequestHandler handler)
        {
            this.requestHandler = handler;
        }

        /// <summary>
        /// Listen for calls for this amount of time
        /// </summary>
        /// <param name="timeOut">The time to wait in miliseconds</param>
        public void ListenForCalls(long timeOut)
        {
            long preTime = DateTime.Now.ToFileTime();
            while (DateTime.Now.ToFileTime() < preTime + (timeOut * 10000) || timeOut == 0)
            {
                // Test if the requesthandler is set
                if (this.requestHandler == null)
                {
                    break;
                }

                // Test if the time has run out
                long timeLeft = ((preTime + (timeOut * 10000)) - DateTime.Now.ToFileTime()) / 10000;
                if (timeLeft > 0) 
                { 
                    // Receive the a packet
                    object data = this.udpMulticast.Receive(timeLeft);
                    
                    // Test if it's a valid packet
                    if (data == null || !data.GetType().Equals(typeof(Packet)))
                    {
                        continue;
                    }

                    // Test if the packet is for the server
                    Packet recPacket = (Packet)data;
                    
                    if (recPacket.GetReceiverId.Equals("server"))
                    {
                        // If the packet has been received before
                        if (this.prevPackets.ContainsKey(recPacket.GetSenderId + "#" + recPacket.GetRequestId))
                        {
                            // Repeat the reply
                            Console.Out.WriteLine("Server repeats");
                            this.udpMulticast.Send(this.prevPackets[recPacket.GetSenderId + "#" + recPacket.GetRequestId]);
                        }
                        else
                        {
                            // The package has not been seen before and we need to generate a response
                            Console.Out.WriteLine("Server sends fresh");
                            string senderId = recPacket.GetSenderId;
                            string requestId = recPacket.GetRequestId;
                            Message resultMessage = this.requestHandler.Invoke(recPacket.GetMessage);
                            Packet resultPacket = new Packet(senderId, "server", requestId, resultMessage);
                            
                            // Add this query and reply to the previous packets
                            this.prevPackets.Add(recPacket.GetSenderId + "#" + recPacket.GetRequestId, resultPacket);
                            this.udpMulticast.Send(resultPacket);
                        }
                    }
                }
            }
        }
    }
}
