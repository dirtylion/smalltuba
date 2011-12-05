// -----------------------------------------------------------------------
// <copyright file="ClientFE.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.Voter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using SmallTuba.Network.Message;
    using System.Text;

    /// <summary>
    /// Network communication in the voting system from the client side
    /// </summary>
    public class VoterNetworkClient
    {
        /// <summary>
        /// The name of this client
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The lower level network class
        /// </summary>
        private readonly ClientFE clientFE;

        /// <summary>
        /// May I have a new VOTER_NETWORK_CLIENT with this name?
        /// </summary>
        /// <param name="name">The name of the client</param>
        public VoterNetworkClient(string name)
        {
            Contract.Requires(name != null);
            this.name = "Client: " + name;
            this.clientFE = new ClientFE(name);
        }

        /// <summary>
        /// May I have all information about the person with this cpr. nr.?
        /// </summary>
        /// <param name="cpr">The cpr number of the person</param>
        /// <returns>The person</returns>
        public Person GetPersonFromCpr(int cpr)
        {
            Message query = new Message(Keyword.GetPersonFromCpr, cpr);
            Message reply = this.clientFE.SendQuery(query, 0);
            if (reply == null)
            {
                return null;
            }

            if (reply.GetValue.GetType().Equals(typeof(Person)))
            {
                return (Person)reply.GetValue;
            }
            
            // The code must never reach this point since we asked for a person
            /// TODO: 
            throw new InvalidCastException();
        }

        /// <summary>
        /// May I have all information about the person with this barcode ID?
        /// </summary>
        /// <param name="id">The id of the person</param>
        /// <returns>The person</returns>
        public Person GetPersonFromId(int id)
        {
            Message query = new Message(Keyword.GetPersonFromId, id);
            Message reply = this.clientFE.SendQuery(query, 0);
            if (reply == null)
            {
                return null;
            }

            if (reply.GetValue.GetType().Equals(typeof(Person)))
            {
                return (Person)reply.GetValue;
            }
            
            // The code must never reach this point since we asked for a person
            /// TODO: 
            throw new InvalidCastException();
        }

        /// <summary>
        /// Has this voter voted?
        /// </summary>
        /// <param name="id">The ID of the voter</param>
        /// <returns>The state of the voter</returns>
        public VoterState HasVoted(int id)
        {
            Message query = new Message(Keyword.HasVoted, id);
            Message reply = this.clientFE.SendQuery(query, 0);
            if (reply == null)
            {
                return null;
            }

            if (reply.GetValue.GetType().Equals(typeof(VoterState)))
            {
                return (VoterState)reply.GetValue;
            }

            // The code must never reach this point since we asked for a person
            /// TODO: 
            throw new InvalidCastException();
        }
        
        /// <summary>
        /// Register that this voter has voted
        /// </summary>
        /// <param name="voterState">The state of the voter</param>
        /// <returns>If the voter was registered</returns>
        public bool RegisterVoter(VoterState voterState)
        {
            Contract.Requires(voterState != null);
            Message query = new Message(Keyword.RegisterVoter, voterState);
            Message reply = this.clientFE.SendQuery(query, 0);
            if (reply == null)
            {
                return false;
            }

            if (reply.GetValue is bool)
            {
                return (bool)reply.GetValue;
            }

            // The code must never reach this point since we asked for a person
            /// TODO: 
            throw new InvalidCastException();
        }

        /// <summary>
        /// Unregister that this voter has voted
        /// </summary>
        /// <param name="id">The id of the voter</param>
        /// <returns>If the voter was unregistered</returns>
        public bool UnregisterVoter(int id)
        {
            Message query = new Message(Keyword.UnregisterVoter, id);
            Message reply = this.clientFE.SendQuery(query, 0);
            if (reply == null)
            {
                return false;
            }

            if (reply.GetValue is bool)
            {
                return (bool)reply.GetValue;
            }

            // The code must never reach this point since we asked for a person
            /// TODO: 
            throw new InvalidCastException();
        }
    }
}