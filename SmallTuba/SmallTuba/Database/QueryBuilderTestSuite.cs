namespace SmallTuba.Database {
	using NUnit.Framework;
	
	[TestFixture()]
	public class QueryBuilderTestSuite {
		private QueryBuilder _query;
		
		[SetUp()]
		public void SetUp () {
			_query = new QueryBuilder();
		}
		
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
		
		[Test()]
		public void TestInsert () {
			_query.SetType("insert");
			_query.SetTable("person");
			_query.SetColumns(new string[] { "id", "firstname", "lastname" });
			_query.SetValues(new string[] { "4", "henrik", "haugbølle" });
			
			string query = _query.Assemble();
			
			Assert.AreEqual("INSERT INTO `person` ( `id`, `firstname`, `lastname` ) VALUES ( '4', 'henrik', 'haugbølle' )", query);
		}
		
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

