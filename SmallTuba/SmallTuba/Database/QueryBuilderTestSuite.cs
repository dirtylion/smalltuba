namespace SmallTuba.Database {
	using NUnit.Framework;
	
	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// Test suite for the QueryBuilder class. The test suite
	/// consists of 4 test, testing the select, update, insert
	/// and delete types of the QueryBuilder.
	/// 
	/// Various parameters such as limits, offsets, orderings,
	/// condition et cetera are tested within these 4 tests.
	/// </summary>
	[TestFixture()]
	public class QueryBuilderTestSuite {
		private QueryBuilder _query;
		
		[SetUp()]
		public void SetUp () {
			_query = new QueryBuilder();
		}
		
		/// <summary>
		/// Assembling and testing a SELECT statement.
		/// 
		/// Also tested: table, columns, condition, 
		/// multiple orderings, limit and offset.
		/// </summary>
		[Test()]
		public void TestSelect () {
			_query.SetType("select");
			_query.SetTable("person");
			_query.SetColumns(new string[] { "id", "firstname", "lastname" });
			_query.AddCondition("firstname = 'henrik'");
			_query.AddOrder("firstname", "desc");
			_query.AddOrder("lastname", "asc");
			_query.SetLimit(10);
			_query.SetOffset(5);
			
			string query = _query.Assemble();
			
			Assert.AreEqual("SELECT `id`, `firstname`, `lastname` FROM `person` WHERE (firstname = 'henrik') ORDER BY `firstname` DESC, `lastname` ASC LIMIT 10 OFFSET 5", query);
		}
		
		/// <summary>
		/// Assembling and testing an UPDATE statement.
		/// 
		/// Also tested: table, columns, values and
		/// single condition.
		/// </summary>
		[Test()]
		public void TestUpdate () {
			_query.SetType("update");
			_query.SetTable("person");
			_query.SetColumns(new string[] { "id", "firstname", "lastname" });
			_query.SetValues(new string[] { "4", "henrik", "haugbølle" });
			_query.AddCondition("firstname = 'henrik'");
			
			string query = _query.Assemble();
			
			Assert.AreEqual("UPDATE `person` SET `id` = '4', `firstname` = 'henrik', `lastname` = 'haugbølle' WHERE (firstname = 'henrik')", query);
		}
		
		/// <summary>
		/// Assembling and testing an INSERT statement.
		/// 
		/// Also tested: table, columns and values.
		/// </summary>
		[Test()]
		public void TestInsert () {
			_query.SetType("insert");
			_query.SetTable("person");
			_query.SetColumns(new string[] { "id", "firstname", "lastname" });
			_query.SetValues(new string[] { "4", "henrik", "haugbølle" });
			
			string query = _query.Assemble();
			
			Assert.AreEqual("INSERT INTO `person` ( `id`, `firstname`, `lastname` ) VALUES ( '4', 'henrik', 'haugbølle' )", query);
		}
		
		/// <summary>
		/// Assembling and testing a DELETE statement.
		/// 
		/// Also tested: table and multiple conditions.
		/// </summary>
		[Test()]
		public void TestDelete () {
			_query.SetType("delete");
			_query.SetTable("person");
			_query.AddCondition("id = 8", "or");
			_query.AddCondition("id = 6");
			
			string query = _query.Assemble();
			
			Assert.AreEqual("DELETE FROM `person` WHERE (id = 8) OR (id = 6)", query);
		}
	}
}

