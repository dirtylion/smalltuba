﻿// -----------------------------------------------------------------------
// <copyright file="ClientFE.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.RPC
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using SmallTuba.Entities;
	using SmallTuba.Network.RequestReply;
	using System.Text;

	/// <summary>
	/// The client side of the network communication in our voting system.
	/// This is the top most level and communication is based on procedure calls
	/// </summary>
	public class VoterClient
	{
		/// <summary>
		/// The name of this client
		/// </summary>
		private string name;

		/// <summary>
		/// The request reply network class
		/// </summary>
		private readonly ClientFrontEnd clientFrontEnd;

        /// <summary>
        /// May I have a new client for the voter network with this name?
        /// </summary>
        /// <param name="name">The name of the client</param>
        public VoterClient(string name)
        {
            this.name = name;
            this.clientFrontEnd = new ClientFrontEnd(name);
        }

		/// <summary>
		/// The name of this client
		/// The name in the underlying architecture is set by the constructor
		/// If the name is changed, it only changes the name received at the VoterServer class
		/// </summary>
		public string Name
		{
			get { return name; }
			set { this.name = value; }
		}

		/// <summary>
		/// May I have all information about the person with this cpr. nr.?
		/// </summary>
		/// <param name="cpr">The cpr number of the person</param>
		/// <returns>The person</returns>
		public Person GetPersonFromCpr(int cpr)
		{
			Contract.Ensures(Connected() ? Contract.Result<Person>() != null : Contract.Result<Person>() == null);
			// The request to send
			Message request = new Message(Keyword.GetPersonFromCpr, name, cpr);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return null;
			}
			// If it was a valid reply
			if (reply is Message && ((Message)reply).GetValue is Person)
			{
				Message replyMessage = (Message) reply;
				return (Person)replyMessage.GetValue;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return null;
		}

		/// <summary>
		/// May I have all information about the person with this barcode ID?
		/// </summary>
		/// <param name="id">The id of the person</param>
		/// <returns>The person</returns>
		public Person GetPersonFromId(int id)
		{
			Contract.Ensures(Connected() ? Contract.Result<Person>() != null : Contract.Result<Person>() == null);
			// The request to send
			Message request = new Message(Keyword.GetPersonFromId, name, id);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return null;
			}
			// If it was a valid reply
			if (reply is Message && ((Message)reply).GetValue is Person)
			{
				Message replyMessage = (Message)reply;
				return (Person)replyMessage.GetValue;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return null;
		}

		/// <summary>
		/// Register that this voter has voted
		/// </summary>
		/// <param name="person">The state of the voter</param>
		/// <returns>If the voter was registered</returns>
		public bool RegisterVoter(Person person)
		{
			Contract.Requires(person != null);
			Contract.Ensures(Connected() ? true : Contract.Result<bool>() == false);
			// The request to send
			Message request = new Message(Keyword.RegisterVoter, name, person);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return false;
			}
			// If it was a valid reply
			if (reply is Message && ((Message)reply).GetValue is bool)
			{
				Message replyMessage = (Message)reply;
				return (bool)replyMessage.GetValue;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return false;
		}

		/// <summary>
		/// Unregister that this voter has voted
		/// </summary>
		/// <param name="person">The id of the voter</param>
		/// <returns>If the voter was unregistered</returns>
		public bool UnregisterVoter(Person person)
		{
			Contract.Requires(person != null);
			Contract.Ensures(Connected() ? true : Contract.Result<bool>() == false);
			// The request to send
			Message request = new Message(Keyword.UnregisterVoter, name, person);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return false;
			}
			// If it was a valid reply
			if (reply is Message && ((Message)reply).GetValue is bool)
			{
				Message replyMessage = (Message)reply;
				return (bool)replyMessage.GetValue;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return false;
		}

		/// <summary>
		/// What are the valid tables for this server?
		/// </summary>
		/// <returns></returns>
		public string[] ValidTables()
		{
			Contract.Ensures(!Connected() ? Contract.Result<string[]>() == null : true);
			// The request to send
			Message request = new Message(Keyword.ValidTables, name, null);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return null;
			}
			// If it was a valid reply
			if (reply is Message && ((Message)reply).GetValue is string[])
			{
				Message replyMessage = (Message)reply;
				return (string[])replyMessage.GetValue;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return null;
		}

		/// <summary>
		/// Are you connected to a server?
		/// </summary>
		/// <returns></returns>
		public bool Connected()
		{
			// The request to send
			Message request = new Message(Keyword.Ping, name, null);
			// The reply received
			object reply = this.clientFrontEnd.SendRequest(request, 5000);
			// If it was a timeout
			if (reply == null)
			{
				return false;
			}
			// If it was a valid reply
			if (reply is Message)
			{
				return true;
			}
			// The server should never send invalid replys, because the request-reply
			// classes ensures that the received reply is a reply for this request
			Contract.Assert(false);
			return false;
		}

		/// <summary>
		/// The name must never be null, or of length 0 since it is used at the server
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(!String.IsNullOrEmpty(name) && name != "server");
		}
	}
}