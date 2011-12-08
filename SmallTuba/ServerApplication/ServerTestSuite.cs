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
			var person = _server.CprToPersonRequestHandler(0123456789);
			
			Assert.That(person.Exists);
			Assert.That(person.DbId == 1);
			Assert.That(person.Firstname == "Henrik");
			Assert.That(person.Lastname == "Haugbølle");
			Assert.That(person.Cpr == 0123456789);
			Assert.That(person.VoterId == 3306);
			Assert.That(person.PollingVenue == "Venue of Awesome");
			Assert.That(person.PollingTable == "Table of Win");
		}
		
		/// <summary>
		/// Testing the CprToPersonRequestHandler method
		/// with a cpr from a non-existing/invalid person.
		/// </summary>
		[Test()]
		public void TestCprToPersonRequestHandlerWithUnexistingPerson() {
			var person = _server.CprToPersonRequestHandler(0711891952);
			
			Assert.That(person.Exists == false);
			Assert.That(person.DbId == 0);
			Assert.That(person.Firstname == "");
			Assert.That(person.Lastname == "");
			Assert.That(person.Cpr == 0);
			Assert.That(person.VoterId == 0);
			Assert.That(person.PollingVenue == "");
			Assert.That(person.PollingTable == "");
		}

		/// <summary>
		/// Testing the IdToPersonRequestHandler method
		/// with a id from a valid, existing person.
		/// </summary>
		[Test()]
		public void TestVoterIdToPersonRequestHandlerWithExistingPerson() {
			var person = _server.VoterIdToPersonRequestHandler(3306);
			
			Assert.That(person.Exists);
			Assert.That(person.DbId == 1);
			Assert.That(person.Firstname == "Henrik");
			Assert.That(person.Lastname == "Haugbølle");
			Assert.That(person.Cpr == 0123456789);
			Assert.That(person.VoterId == 3306);
			Assert.That(person.PollingVenue == "Venue of Awesome");
			Assert.That(person.PollingTable == "Table of Win");
		}

		/// <summary>
		/// Testing the IdToPersonRequestHandler method
		/// with a id from a valid, existing person.
		/// </summary>
		[Test()]
		public void TestVoterIdToPersonRequestHandlerWithUnexistingPerson() {
			var person = _server.VoterIdToPersonRequestHandler(669);

			Assert.That(person.Exists == false);
			Assert.That(person.DbId == 0);
			Assert.That(person.Firstname == "");
			Assert.That(person.Lastname == "");
			Assert.That(person.Cpr == 0);
			Assert.That(person.VoterId == 0);
			Assert.That(person.PollingVenue == "");
			Assert.That(person.PollingTable == "");
		}

		/// <summary>
		/// Test registration of a voter/person which has
		/// not voted before and is a valid voter.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithExistingNonVotePerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 1 } });
			
			var person = personEntity.ToObject();

			Assert.That(_server.RegisterVoteRequestHandler(person));

			var logEntity = personEntity.GetMostRecentLog();
			logEntity.Delete();
		}

		/// <summary>
		/// Test registration of a voter/person which has
		/// voted before but is a valid voter.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithExistingVotePerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 2 } });
			
			var person = personEntity.ToObject();

			Assert.That(!_server.RegisterVoteRequestHandler(person));
		}

		/// <summary>
		/// Test registration with a non-existing person.
		/// </summary>
		[Test()]
		public void TestRegisterVoteRequestHandlerWithUnexistingPerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 669 } });
			
			var person = personEntity.ToObject();

			Assert.That(!_server.RegisterVoteRequestHandler(person));
		}

		/// <summary>
		/// Test unregistration of a person who have not
		/// voted before but is a valid voter.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWithExistingNonVotePerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 1 } });
			
			var person = personEntity.ToObject();

			Assert.That(!_server.UnregisterVoteRequestHandler(person));
		}

		/// <summary>
		/// Test unregistration of a person who have 
		/// voted before and is a valid voter.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWithExistingVotePerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 2 } });
			
			var person = personEntity.ToObject();

			Assert.That(_server.UnregisterVoteRequestHandler(person));

			var logEntity = personEntity.GetMostRecentLog();
			logEntity.Delete();
		}

		/// <summary>
		/// Test unregistration with a non-existing person.
		/// </summary>
		[Test()]
		public void TestUnregisterVoteRequestHandlerWitUnexistingPerson() {
			var personEntity = new PersonEntity();
			personEntity.Load(new Hashtable { { "id", 669 } });
			
			var person = personEntity.ToObject();

			Assert.That(!_server.UnregisterVoteRequestHandler(person));
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