namespace ServerApplication {
	using System.Collections;
	using System.Collections.Generic;

	using NUnit.Framework;
	
	using SmallTuba;
	using SmallTuba.Entities;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-08</version>
	/// <summary>
	/// Testing the interface of the server. Making sure
	/// that the reponses to the request is the correct
	/// data to be send over the network et cetera.
	/// </summary>
	[TestFixture()]
	public class ServerTestSuite {
		private Server _server;
		
		/// <summary>
		/// Initialize an instance of the Server class
		/// to test against.
		/// </summary>
		[TestFixtureSetUp()]
		public void TestSetUp() {
			_server = new Server();

			Debug.ExternalDataSources = true;
		}

		[TestFixtureTearDown()]
		public void TestTearDown() {
			Debug.ExternalDataSources = false;
		} 

		/// <summary>
		/// Testing the CprToPersonRequestHandler method
		/// with a cpr from a valid, existing person.
		/// </summary>
		[Test()]
		public void TestCprToPersonRequestHandlerWithExistingPerson() {
			var personState = _server.CprToPersonRequestHandler(0123456789);
			
			Assert.That(personState.Exists);
			Assert.That(personState.Id == 1);
			Assert.That(personState.Firstname == "Henrik");
			Assert.That(personState.Lastname == "Haugbølle");
			Assert.That(personState.Cpr == 0123456789);
			Assert.That(personState.Barcode == 3306);
			Assert.That(personState.PollingVenue == "Venue of Awesome");
			Assert.That(personState.PollingTable == "Table of Win");
		}
		
		/// <summary>
		/// Testing the CprToPersonRequestHandler method
		/// with a cpr from a non-existing/invalid person.
		/// </summary>
		[Test()]
		public void TestCprToPersonRequestHandlerWithUnexistingPerson() {
			var personState = _server.CprToPersonRequestHandler(0711891952);
			
			Assert.That(personState.Exists == false);
			Assert.That(personState.Id == 0);
			Assert.That(personState.Firstname == "");
			Assert.That(personState.Lastname == "");
			Assert.That(personState.Cpr == 0);
			Assert.That(personState.Barcode == 0);
			Assert.That(personState.PollingVenue == "");
			Assert.That(personState.PollingTable == "");
		}

		/// <summary>
		/// Testing the IdToPersonRequestHandler method
		/// with a id from a valid, existing person.
		/// </summary>
		[Test()]
		public void TestBarcodeToPersonRequestHandlerWithExistingPerson() {
			var personState = _server.BarcodeToPersonRequestHandler(3306);
			
			Assert.That(personState.Exists);
			Assert.That(personState.Id == 1);
			Assert.That(personState.Firstname == "Henrik");
			Assert.That(personState.Lastname == "Haugbølle");
			Assert.That(personState.Cpr == 0123456789);
			Assert.That(personState.Barcode == 3306);
			Assert.That(personState.PollingVenue == "Venue of Awesome");
			Assert.That(personState.PollingTable == "Table of Win");
		}

		/// <summary>
		/// Testing the IdToPersonRequestHandler method
		/// with a id from a valid, existing person.
		/// </summary>
		[Test()]
		public void TestBarcodeToPersonRequestHandlerWithUnexistingPerson() {
			var personState = _server.BarcodeToPersonRequestHandler(669);

			Assert.That(personState.Exists == false);
			Assert.That(personState.Id == 0);
			Assert.That(personState.Firstname == "");
			Assert.That(personState.Lastname == "");
			Assert.That(personState.Cpr == 0);
			Assert.That(personState.Barcode == 0);
			Assert.That(personState.PollingVenue == "");
			Assert.That(personState.PollingTable == "");
		}

		/// <summary>
		/// Test registration of a voter/person which has
		/// not voted before and is a valid voter.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithExistingNonVotePerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 1 } });
			
			var personState = person.ToStateObject();

			Assert.That(_server.RegisterVoteRequestHandler(personState));

			var log = person.GetMostRecentLog();
			log.Delete();
		}

		/// <summary>
		/// Test registration of a voter/person which has
		/// voted before but is a valid voter.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithExistingVotePerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 2 } });
			
			var personState = person.ToStateObject();

			Assert.That(!_server.RegisterVoteRequestHandler(personState));
		}

		/// <summary>
		/// Test registration with a non-existing person.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithUnexistingPerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 669 } });
			
			var personState = person.ToStateObject();

			Assert.That(!_server.RegisterVoteRequestHandler(personState));
		}

		/// <summary>
		/// Test unregistration of a person who have not
		/// voted before but is a valid voter.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWithExistingNonVotePerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 1 } });
			
			var personState = person.ToStateObject();

			Assert.That(!_server.UnregisterVoteRequestHandler(personState));
		}

		/// <summary>
		/// Test unregistration of a person who have 
		/// voted before and is a valid voter.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWithExistingVotePerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 2 } });
			
			var personState = person.ToStateObject();

			Assert.That(_server.UnregisterVoteRequestHandler(personState));

			var log = person.GetMostRecentLog();
			log.Delete();
		}

		/// <summary>
		/// Test unregistration with a non-existing person.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWitUnexistingPerson() {
			var person = new Person();
			person.Load(new Hashtable { { "id", 669 } });
			
			var personState = person.ToStateObject();

			Assert.That(!_server.UnregisterVoteRequestHandler(personState));
		}

		/// <summary>
		/// Test that the server returns the correct tables
		/// from the data source and that it does not return
		/// the same table twice.
		/// </summary>
		[Test()]
		public void TestValidTableRequestHandler() {
			var tables = _server.ValidTableRequestHandler();

			var tablesList = new List<string>(tables);

			Assert.That(tablesList.Count == 3);
			Assert.That(tablesList.Contains("Table of Win"));
			Assert.That(tablesList.Contains("Table of Fish"));
			Assert.That(tablesList.Contains("Table of Calmness"));
		}
	}
}