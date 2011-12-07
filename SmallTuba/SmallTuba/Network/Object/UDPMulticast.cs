// -----------------------------------------------------------------------
// <copyright file="UDPMulticast.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.Object
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
 
    /// <summary>
    /// Class used for multicasting data
    /// </summary>
    public class UDPMulticast
    {
        /// <summary>
        /// The Udpclient that wraps all the socket programming nicely
        /// </summary>
        private readonly UdpClient client;

        /// <summary>
        /// Where we send the packages to
        /// </summary>
        private readonly IPEndPoint sendPoint;

        /// <summary>
        /// Where the packages come from
        /// </summary>
        private IPEndPoint recPoint;

        /// <summary>
        /// May i have a new multicast client?
        /// </summary>
        /// <param name="server">If the client should be server or client 0/1</param>
        public UDPMulticast(int server)
        {
            // Creates a new client initialized for port 5000/5001
            this.client = new UdpClient(5000 + 1 - server);
            
            // The multicast adress
            IPAddress ip = IPAddress.Parse("224.5.6.7"); 
            
            // Where we send the packages to
            this.sendPoint = new IPEndPoint(ip, 5000 + server);

            // Where we recieve them from
            this.recPoint = null;

            // Join the multicast group
            this.client.JoinMulticastGroup(ip);
        }

        /// <summary>
        /// Send this object
        /// </summary>
        /// <param name="o">The object to send</param>
        public void Send(object o)
        {
            Contract.Requires(o != null);
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, o);
            var data = ms.ToArray();
            this.client.Send(data.ToArray(), data.Length, this.sendPoint);
        }

        /// <summary>
        /// May I have an object if one is in queue or arrives within this timeframe?
        /// </summary>
        /// <param name="timeOut">The time to wait</param>
        /// <returns>The received object</returns>
        public object Receive(long timeOut)
        {
            var preTime = DateTime.Now.ToFileTime();
            while (DateTime.Now.ToFileTime() < preTime + (timeOut * 10000) || timeOut == 0)
            {
                if (this.client.Available > 0)
                {
                    byte[] data = this.client.Receive(ref this.recPoint);
                    var ms = new MemoryStream();
                    var bf = new BinaryFormatter();
                    ms.Write(data, 0, data.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        return bf.Deserialize(ms);
                    }
                    catch (System.Runtime.Serialization.SerializationException)
                    {
                        // The received input was not an object
                        // Maybe someone else is multicasting to this port
                    }
                }
                else
                {
                    // Sleeps for 10 miliseconds
                    System.Threading.Thread.Sleep(10);
                }
            }

            return null;
        }
    }
}
