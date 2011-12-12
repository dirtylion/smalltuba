namespace SmallTubaTestSuite {
	using System.Collections;
	using NUnit.Framework;
	using SmallTuba.Database;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// TestSuite for the Connector class. To use the
	/// class one most make sure that the Connector
	/// can actually connect to a valid MySQL database.
	/// See the Connector class for more information.
	/// 
	/// The database must contain a table called
	/// "PersonTestSuite" and the following columns:
	/// 
	/// id INT 11
	/// firstname VARCHAR 100
	/// 
	/// And the data as represented below:
	/// 
	/// id	firstname
	/// 1	Henrik
	/// 2	Christian
	/// 3	Kåre
	/// 
	/// Otherwise the test suite will fail the tests.
	/// </summary>
	[TestFixture()]
	public class ConnectorTestSuite {
		private Connector _connector;

		/// <summary>
		/// Instantiate the Connector and call the
		/// Connect() method to connect to the database.
		/// </summary>
		[SetUp()]
		public void SetUp() {
			_connector = Connector.GetConnector();
			_connector.Connect();
		}

		/// <summary>
		/// Close the connection to the database by 
		/// calling the Disconnect() method.
		/// </summary>
		[TearDown()]
		public void TearDown() {
			_connector.Disconnect();
		} 

		/// <summary>
		/// Execute a SELECT query towards the database. Fetch
		/// all the content from the PersonTestSuite table and
		/// check that both id and firstnames matches.
		/// </summary>
		[Test()]
		public void TestExecuteQuery() {
			var results = _connector.ExecuteQuery("SELECT * FROM `PersonTestSuite`;");
			
			Assert.That(((int) ((Hashtable) results[0])["id"]) == 1);
			Assert.That(((string) ((Hashtable) results[0])["firstname"]) == "Henrik");

			Assert.That(((int) ((Hashtable) results[1])["id"]) == 2);
			Assert.That(((string) ((Hashtable) results[1])["firstname"]) == "Christian");

			Assert.That(((int) ((Hashtable) results[2])["id"]) == 3);
			Assert.That(((string) ((Hashtable) results[2])["firstname"]) == "Kåre");
		}

		/// <summary>
		/// Execute an UPDATE statement towards the database,
		/// and check with a SELECT statement that the update
		/// query was executed correctly.
		/// 
		/// Afterwards reverse the update and check it, so that
		/// it is possible to re-run the test.
		/// </summary>
		[Test()]
		public void TestExecuteNoneQuery() {
			int lastID = _connector.ExecuteNoneQuery("UPDATE PersonTestSuite SET `firstname` = 'Henrik Haugbølle' WHERE `id` = 1 LIMIT 1;");

			var results = _connector.ExecuteQuery("SELECT * FROM `PersonTestSuite` WHERE `id` = 1 LIMIT 1;");
			
			Assert.That(((int) ((Hashtable) results[0])["id"]) == 1);
			Assert.That(((string) ((Hashtable) results[0])["firstname"]) == "Henrik Haugbølle");

			lastID = _connector.ExecuteNoneQuery("UPDATE PersonTestSuite SET `firstname` = 'Henrik' WHERE `id` = 1 LIMIT 1;");
			
			results = _connector.ExecuteQuery("SELECT * FROM `PersonTestSuite` WHERE `id` = 1 LIMIT 1;");
			
			Assert.That(((int) ((Hashtable) results[0])["id"]) == 1);
			Assert.That(((string) ((Hashtable) results[0])["firstname"]) == "Henrik");
		}
	}
}