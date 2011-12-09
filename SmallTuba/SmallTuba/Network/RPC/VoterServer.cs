namespace SmallTuba.Network.RPC
{
	using System;
	using System.Diagnostics.Contracts;
	using SmallTuba.Entities;
    using SmallTuba.Network.RequestReply;

	/// <summary>
	/// The server side of the network communication in our voting system.
	///	This is the top most level and communication is based on procedure calls.
	/// </summary>
	public class VoterServer
	{
		/// <summary>
		/// The name of the server
		/// </summary>
		private string name;

		/// <summary>
        /// The request reply network class
		/// </summary>
		private ServerFrontEnd serverFrontEnd;

		/// <summary>
		/// Invoke this function that returns a person when asked about a person from a cpr.nr.
		/// </summary>
		private CprToPersonRequest cprToPersonRequest;

		/// <summary>
		/// Invoke this function that returns a person when asked about a person from a barcode ID
		/// </summary>
		private VoterIdToPersonRequest voterIdToPersonRequest;

		/// <summary>
		/// Invoke this function when asked about registering a user
		/// </summary>
		private RegisterVoteRequest registerVoteRequest;

		/// <summary>
		/// Invoke this function when asked about unregistering a user
		/// </summary>
		private UnregisterVoteRequest unregisterVoteRequest;

        /// <summary>
        /// Invoke this function when asked about valid tables
        /// </summary>
        private ValidTableRequest validTableRequest;

		/// <summary>
        /// May I have a new server for the voter network with this name?
		/// </summary>
		/// <param name="name">The name of the server</param>
		public VoterServer(string name)
		{
			Contract.Requires(name != null);
			this.name = "Server: " + name;
			this.serverFrontEnd = new ServerFrontEnd();
			this.serverFrontEnd.SetRequestHandler(this.RequestHandler);
		}

		/// <summary>
		/// A type of a function to invoke when a request for a person is made
		/// </summary>
	    /// <param name="clientName">The name of the client making the call</param>
        /// <param name="cpr">The cpr. nr.</param>
		/// <returns>The person</returns>
		public delegate Person CprToPersonRequest(string clientName, int cpr);

		/// <summary>
		/// A type of a function to invoke when a request for a person is made
		/// </summary>
        /// <param name="clientName">The name of the client making the call</param>
        /// <param name="id">The id</param>
		/// <returns>The person</returns>
        public delegate Person VoterIdToPersonRequest(string clientName, int id);

		/// <summary>
		/// A type of a function to invoke when a request for registering a voter is made
		/// </summary>
        /// <param name="clientName">The name of the client making the call</param>
        /// <param name="person">The person to register</param>
		/// <returns>If the voter was registered</returns>
        public delegate bool RegisterVoteRequest(string clientName, Person person);

		/// <summary>
		/// A type of a function to invoke when a request for unregistering a voter is made
		/// </summary>
        /// <param name="clientName">The name of the client making the call</param>
        /// <param name="person">The person to unregister</param>
		/// <returns>If the voter was unregistered</returns>
        public delegate bool UnregisterVoteRequest(string clientName, Person person);

        /// <summary>
        /// A type of a function to invoke when a request for valid tables is made
        /// </summary>
        /// <param name="clientName">The name of the client making the call</param>
        /// <returns>The valid tables</returns>
		public delegate string[] ValidTableRequest(string clientName);

		/// <summary>
		/// Invoke this function that returns a person when asked about a person from a cpr.nr.
		/// </summary>
		/// <param name="function">The function</param>
		public void SetCprToPersonRequest(CprToPersonRequest function)
		{
			this.cprToPersonRequest = function;
		}

		/// <summary>
		/// Invoke this function that returns a person when asked about a person from a barcode ID
		/// </summary>
		/// <param name="function">The function</param>
		public void SetVoterIdToPersonRequest(VoterIdToPersonRequest function)
		{
			this.voterIdToPersonRequest = function;
		}

		/// <summary>
		/// Invoke this function when asked about registering a user
		/// </summary>
		/// <param name="function">The function</param>
		public void SetRegisterVoteRequest(RegisterVoteRequest function)
		{
			this.registerVoteRequest = function;
		}

		/// <summary>
		/// Invoke this function when asked about unregistering a user
		/// </summary>
		/// <param name="function">The function</param>
		public void SetUnregisterVoteRequest(UnregisterVoteRequest function) 
		{
			this.unregisterVoteRequest = function;
		}

        /// <summary>
        /// Invoke this function when asked about valid tables
        /// </summary>
        /// <param name="function">The function</param>
		public void SetValidTableRequest(ValidTableRequest function)
		{
			this.validTableRequest = function;
		}

		/// <summary>
		/// Listen for calls for this amount of time
		/// </summary>
		/// <param name="timeOut">The amount of time in milliseconds, if zero it waits forever</param>
		public void ListenForCalls(int timeOut)
		{
            Contract.Requires(timeOut >= 0);

            this.serverFrontEnd.ListenForCalls(timeOut);
		}

		/// <summary>
		/// This function is called when a request from a client is received
		/// </summary>
		/// <param name="o">The request from the client</param>
		/// <returns>The reply for the client</returns>
		private object RequestHandler(object o)
		{
            // This should not be possible
            // Only packets from a SmallTuba.Network.RequestReply.ClientFrontEnd will be received
            if(!(o is Message))
            {
                Contract.Assert(false);
                return null;
            }

            Message request = (Message) o;
			Keyword keyword = request.GetKeyword;
			// Test which procedure is to be invoked
            switch (keyword)
			{
				case Keyword.GetPersonFromCpr:
					if (request.GetValue is int && this.cprToPersonRequest != null)
					{
						Person person = this.cprToPersonRequest.Invoke(request.GetSender, (int)request.GetValue);
						return new Message(keyword, name, person);
					}
                    // The execution should never reach this point unless VoterClient has errors
                    // or the server is not listening for this kind of requests
                    Contract.Assert(false);
			        return null;

				case Keyword.GetPersonFromId:
					if (request.GetValue is int && this.voterIdToPersonRequest != null)
					{
                        Person person = this.voterIdToPersonRequest.Invoke(request.GetSender, (int)request.GetValue);
						return new Message(keyword, name, person);
					}
                    // The execution should never reach this point unless VoterClient has errors
                    // or the server is not listening for this kind of requests
                    Contract.Assert(false);
                    return null;

				case Keyword.RegisterVoter:
					if (request.GetValue.GetType().Equals(typeof(Person)) && this.registerVoteRequest != null)
					{
                        bool b = this.registerVoteRequest.Invoke(request.GetSender, (Person)request.GetValue);
						return new Message(keyword, name, b);
					}
                    // The execution should never reach this point unless VoterClient has errors
                    // or the server is not listening for this kind of requests
                    Contract.Assert(false);
                    return null;
				
				case Keyword.UnregisterVoter:
					if (request.GetValue.GetType().Equals(typeof(Person)) && this.unregisterVoteRequest != null)
					{
                        bool b = this.unregisterVoteRequest.Invoke(request.GetSender, (Person)request.GetValue);
						return new Message(keyword, name, b);
					}
                    // The execution should never reach this point unless VoterClient has errors
                    // or the server is not listening for this kind of requests
                    Contract.Assert(false);
                    return null;
				
				case Keyword.ValidTables:
                    if(this.validTableRequest != null)
                    {
                        string[] arr = this.validTableRequest.Invoke(request.GetSender);
                        return new Message(keyword, name, arr);
                    }
                    // The execution should never reach this point unless VoterClient has errors
                    // or the server is not listening for this kind of requests
			        Contract.Assert(false);
			        return null;

				case Keyword.Ping:
					// An empty message indicating that the server is listening
                    return new Message(keyword, name, null);
				
				default:
                    // Should not happen
			        Contract.Assert(false);
					return null;
			}
		}
	}
}
