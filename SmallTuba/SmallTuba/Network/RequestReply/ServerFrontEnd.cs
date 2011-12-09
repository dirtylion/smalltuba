﻿// -----------------------------------------------------------------------
// <copyright file="ServerCom.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.RequestReply
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using SmallTuba.Network.UDP;
    using System.Text;

    /// <summary>
    /// This class listens for request for the server and replies.
	/// This class only receives request adressed to the server
    ///</summary>
    public class ServerFrontEnd
    {
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
        /// May I have a new server front end?
        /// </summary>
        public ServerFrontEnd()
        {
            // Create a new udpMulticast as a server
            this.udpMulticast = new UDPMulticast(0);
            this.prevPackets = new Dictionary<string, Packet>();
        }

        /// <summary>
        /// A type of function to invoke when a message is received
        /// </summary>
        /// <param name="request">The request from the client</param>
        /// <returns>The result to the client</returns>
        public delegate object RequestHandler(object request);

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
        /// If a request with the same request id is repeated, the reply is repeated
        /// </summary>
        /// <param name="timeOut">The time to wait in miliseconds</param>
        public void ListenForCalls(long timeOut)
        {
            Contract.Requires(timeOut >= 0);

            // If the server should listen for requests forever
            bool runForever = timeOut == 0;
            // The time before the loop was entered
            long preTime = DateTime.Now.ToFileTime();
            // The time left before timeout
            long timeLeft;
            
            // Listen
            while (runForever || DateTime.Now.ToFileTime() < preTime + (timeOut * 10000))
            {
                // Test if the requesthandler is set
                if (this.requestHandler == null)
                {
                    // If the eventhandler is not set packages would be dropped therefore we break
                    break;
                }
                
                // Receive the packet
                object data;
                if (runForever)
                {
                    // The udpMulticast is allowed to block forever
                    data = udpMulticast.Receive(0);
                }
                else
                {
                    // The udpMulticast is allowed to block for the time left
                    timeLeft = ((preTime + (timeOut * 10000)) - DateTime.Now.ToFileTime()) / 10000;
                    data = this.udpMulticast.Receive(timeLeft);
                }
                    
                // Test if it's a valid packet
                if (data == null || !(data is Packet))
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
                        object resultMessage = this.requestHandler.Invoke(recPacket.GetMessage);
                        Packet resultPacket = new Packet(senderId, "server", requestId, resultMessage);
                        
                        // Add this request and reply to the list of previous packets
                        this.prevPackets.Add(recPacket.GetSenderId + "#" + recPacket.GetRequestId, resultPacket);
                        this.udpMulticast.Send(resultPacket);
                    }
                }
            }
        }
    }
}