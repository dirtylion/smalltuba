namespace SmallTuba.Network.Voter
{
	using System;
	using System.Diagnostics.Contracts;
	using SmallTuba.Entities;
	using SmallTuba.Network.Message;

	/// <summary>
	/// Network communication in the voting system from the server side
	/// </summary>
	public class VoterNetworkServer
	{
		/// <summary>
		/// The name of the server
		/// </summary>
		private string name;

		/// <summary>
		/// The lower level network communication object
		/// </summary>
		private ServerFE serverFe;

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

		private ValidTableRequest validTableRequest;

		/// <summary>
		/// May I have a new VOTER_NETWORK_SERVER with this name?
		/// </summary>
		/// <param name="name">The name of the server</param>
		public VoterNetworkServer(string name)
		{
			Contract.Requires(name != null);
			this.name = "Server: " + name;
			this.serverFe = new ServerFE(name);
			this.serverFe.SetRequestHandler(this.RequestHandler);
		}

		/// <summary>
		/// A type of a function to invoke when a request for a person is made
		/// </summary>
		/// <param name="cpr">The cpr. nr.</param>
		/// <returns>The person</returns>
		public delegate Person CprToPersonRequest(int cpr);

		/// <summary>
		/// A type of a function to invoke when a request for a person is made
		/// </summary>
		/// <param name="id">The id</param>
		/// <returns>The person</returns>
		public delegate Person VoterIdToPersonRequest(int id);

		/// <summary>
		/// A type of a function to invoke when a request for registering a voter is made
		/// </summary>
		/// <param name="voterState">The state of the voret</param>
		/// <returns>If the voter was registered</returns>
		public delegate bool RegisterVoteRequest(Person person);

		/// <summary>
		/// A type of a function to invoke when a request for unregistering a voter is made
		/// </summary>
		/// <param name="id">The id of the voter</param>
		/// <returns>If the voter was unregistered</returns>
		public delegate bool UnregisterVoteRequest(Person person);

		public delegate string[] ValidTableRequest();

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

		public void SetValidTableRequest(ValidTableRequest function)
		{
			this.validTableRequest = function;
		}

		/// <summary>
		/// Listen for calls for this amount of time
		/// </summary>
		/// <param name="timeOut">The amount of time</param>
		public void ListenForCalls(int timeOut)
		{
			this.serverFe.ListenForCalls(timeOut);
		}

		/// <summary>
		/// This function is called when there is a packet form the network
		/// </summary>
		/// <param name="query">The query from the client</param>
		/// <returns>A packet response for the client</returns>
		private Message RequestHandler(Message query)
		{
			Keyword keyword = query.GetKeyword;
			switch (keyword)
			{
				case Keyword.GetPersonFromCpr:
					if (query.GetValue is int && this.cprToPersonRequest != null)
					{
						Person person = this.cprToPersonRequest.Invoke((int)query.GetValue);
						return new Message(keyword, person);
					}
                    System.Diagnostics.Contracts.Contract.Assert(false);
			        return null;

				case Keyword.GetPersonFromId:
					if (query.GetValue is int && this.voterIdToPersonRequest != null)
					{
						Person person = this.voterIdToPersonRequest.Invoke((int)query.GetValue);
						return new Message(keyword, person);
					}
                    System.Diagnostics.Contracts.Contract.Assert(false);
                    return null;

				case Keyword.RegisterVoter:
					if (query.GetValue.GetType().Equals(typeof(Person)) && this.registerVoteRequest != null)
					{
						Person person = this.registerVoteRequest.Invoke((Person)query.GetValue);
						return new Message(keyword, person);
					}
                    System.Diagnostics.Contracts.Contract.Assert(false);
                    return null;
				
				case Keyword.UnregisterVoter:
					if (query.GetValue.GetType().Equals(typeof(Person)) && this.unregisterVoteRequest != null)
					{
						Person person = this.unregisterVoteRequest.Invoke((Person)query.GetValue);
						return new Message(keyword, person);
					}
                    System.Diagnostics.Contracts.Contract.Assert(false);
                    return null;
				
				case Keyword.ValidTables:
					string[] arr = this.validTableRequest.Invoke();
					return new Message(keyword, arr);
				
				case Keyword.Ping:
					return new Message(keyword, null);
				
				default:
					return null;
			}
		}
	}
}
