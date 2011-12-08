namespace ServerApplication {
	using System;
	using System.Collections;
	
	using SmallTuba.Database;
	using SmallTuba.Entities;
	using SmallTuba.Network.Voter;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-08</version>
	/// <summary>
	/// The Server class handles request from the ClientApplication,
	/// by utilizing the VoterNetworkServer and entity classes.
	/// 
	/// The Server will listen forever (or until external process
	/// kill) for requests, and upon request it will utilize a
	/// callback assigned to the underlying VoterNetworkServer
	/// which requests data from the external data source by
	/// utilizing the entity and QueryBuilder classes.
	/// </summary>
	class Server {
		private VoterNetworkServer voterNetWorkServer;


		/// <summary>
		/// Construct a new server and instantiate and instance
		/// of the VoterNetworkServer identified as "primary".
		/// </summary>
		public Server() {
			voterNetWorkServer = new VoterNetworkServer("Primary");
		}

		/// <summary>
		/// Boot the server by attaching the proper callback functions
		/// to the listeners on the VoterNetworkServer object.
		/// </summary>
		public void Start() {
			Console.WriteLine("server start");
			
			voterNetWorkServer.SetCprToPersonRequest(CprToPersonRequestHandler);
			voterNetWorkServer.SetBarcodeToPersonRequest(BarcodeToPersonRequestHandler);

			voterNetWorkServer.SetRegisterVoteRequest(RegisterVoteRequestHandler);
			voterNetWorkServer.SetUnregisterVoteRequest(UnregisterVoteRequestHandler);

			voterNetWorkServer.SetValidTableRequest(ValidTableRequestHandler);

			voterNetWorkServer.ListenForCalls(0);
		}

		/// <summary>
		/// Handle a request for a PersonState object by CPR.
		/// 
		/// Takes a CPR number as parameter and constructs the
		/// proper Person entity. If the entity does not exists
		/// a PersonState object will still be passed back,
		/// but the Exist property of the object will return false.
		/// </summary>
		/// <param name="cpr">The CPR number to search for a person by.</param>
		/// <returns>A PersonState object filled with information from the Person entity.</returns>
		public PersonState CprToPersonRequestHandler(int cpr) {
			Console.WriteLine("CprToPersonRequestHandler");

			var person = new Person();
			person.Load(new Hashtable { { "cpr", cpr } });

			if (person.Exists()) {
				Console.WriteLine("Person exists, Id: "+person.Id);
			}

			return person.ToStateObject();
		}

		/// <summary>
		/// Handle a request for a PersonState object by barcode.
		/// 
		/// Takes a barcode number as parameter and constructs the
		/// proper Person entity. If the entity does not exists
		/// a PersonState object will still be passed back,
		/// but the Exist property of the object will return false.
		/// </summary>
		/// <param name="barcode">The barcode number to search for a person by.</param>
		/// <returns>A PersonState object filled with information from the Person entity.</returns>
		public PersonState BarcodeToPersonRequestHandler(int barcode) {
			Console.WriteLine("BarcodeToPersonRequestHandler");

			var person = new Person();
			person.Load(new Hashtable { { "barcode", barcode } });
			
			if (person.Exists()) {
				Console.WriteLine("Person exists, Id: "+person.Id);
			}

			return person.ToStateObject();
		}

		/// <summary>
		/// Handle a request to register a person.
		/// 
		/// The method takes a PersonState object sent from
		/// a client application, which contains information
		/// about the person who should be registered.
		/// 
		/// If the person does not exists or if the person
		/// already voted, the method will return false.
		/// Otherwise the method will return true meaning
		/// that the registration successed and that a log
		/// entity in the database was created.
		/// </summary>
		/// <param name="personState">The person to be registered.</param>
		/// <returns>A boolean value determining if the request failed or successed.</returns>
		public bool RegisterVoteRequestHandler(PersonState personState) {
			Console.WriteLine("RegisterVoteRequestHandler");
			var person = new Person();
			person.Load(new Hashtable { { "id", personState.Id } });

			if (person.Exists() && !person.Voted) {
				Console.WriteLine("Person exists, Id: "+person.Id);

				var log = new Log {
					PersonId = person.Id,
					Action = "register",
					Client = "Client 8",
					PollingTable = "8",
					Timestamp = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
				};

				log.Save();
				Console.WriteLine("Log saved, Id: "+log.Id);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Handle a request to unregister a person.
		/// 
		/// The method takes a PersonState object sent from
		/// a client application, which contains information
		/// about the person who should be registered.
		/// 
		/// If the person does not exists or if the person
		/// already voted, the method will return false.
		/// Otherwise the method will return true meaning
		/// that the unregistration successed and that a log
		/// entity in the database was created.
		/// </summary>
		/// <param name="personState">The person to be unregistrated.</param>
		/// <returns>A boolean value determining if the request failed or successed.</returns>
		public bool UnregisterVoteRequestHandler(PersonState personState) {
			Console.WriteLine("UnregisterVoteRequestHandler");
			var person = new Person();
			person.Load(new Hashtable { { "id", personState.Id } });

			if (person.Exists() && person.Voted) {
				Console.WriteLine("Person exists, Id: "+person.Id);

				var log = new Log {
					PersonId = person.Id,
					Action = "unregister",
					Client = "Client 8",
					PollingTable = "8",
					Timestamp = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
				};

				log.Save();
				Console.WriteLine("Log saved, Id: "+log.Id);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the available tables at a polling venue
		/// for a client application to choose between.
		/// </summary>
		/// <returns>The available tables for the client to choose as a string array.</returns>
		public string[] ValidTableRequestHandler() {
			var queryBuilder = new QueryBuilder();
			queryBuilder.SetType("select");
			queryBuilder.SetTable("Person");
			queryBuilder.SetColumns(new [] { "polling_table" } );
			queryBuilder.SetGroupBy("polling_table");

			var results = queryBuilder.ExecuteQuery();
			var tables = new string [results.Count];

			var i = 0;
			foreach (var result in results) {
				tables[i++] = (string) ((Hashtable) result)["polling_table"];
			}

			return tables;
		}

		/// <summary>
		/// The main method starting the thread for the
		/// server to run at and instantiating and starting
		/// an instance of the Server class.
		/// </summary>
		/// <param name="args">Does not use the application arguments.</param>
		static void Main(string[] args) {
			Console.WriteLine("start");

			var server = new Server();
			server.Start();

			Console.WriteLine("end");
			Console.ReadKey();
		}
	}
}
