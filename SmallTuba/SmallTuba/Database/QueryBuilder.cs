namespace SmallTuba.Database {
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics.Contracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// The QueryBuilder is used to assemble MySQL queries from
	/// parameters set by the range of given setter methods.
	/// 
	/// It is also capable of executing the assembled queries
	/// towards a database using an instance of the Connector class.
	/// </summary>
	/// <example>
	/// queryBuilder = new QueryBuilder();
	/// queryBuilder.SetType("select");
	/// queryBuilder.SetTable("person");
	/// queryBuilder.SetColumns(new [] { "id", "firstname" });
	/// queryBuilder.AddCondition("`id` = 1");
	/// queryBuilder.SetLimit(1);
	/// 
	/// string result = queryBuilder.Assemble();
	/// 
	/// // will produce:
	/// // SELECT `id`, `firstname` FROM `Person` WHERE (`id` = 1) LIMIT 1
	/// </example>
	public class QueryBuilder {
		private string _type;
		private string _table;
		private ArrayList _columns;
		private ArrayList _values;
		private OrderedDictionary _conditions;
		private int _limit;
		private int _offset;
		private OrderedDictionary _orders;

		private string _query;

		private Connector _connector;

		/// <summary>
		/// Construct the QueryBuilder.
		/// 
		/// Will initialize an instance of the Connector class
		/// and call the Connect() method of this.
		/// 
		/// Also calls the method Clear() which at this point
		/// works as a field initializer.
		/// </summary>
		public QueryBuilder() {
			_connector = new Connector();
			_connector.Connect();

			Clear();
		}

		/// <summary>
		/// Resets the parameters to their default values.
		/// The default type of the QueryBuilder is "select".
		/// </summary>
		private void Clear() {
			_type = "select";
			_table = "";
			_columns = new ArrayList();
			_values = new ArrayList();
			_conditions = new OrderedDictionary();
			_limit = -1;
			_offset = -1;
			_orders = new OrderedDictionary();
		}

		/// <summary>
		/// Set the type of the query.
		/// </summary>
		/// <param name="type">The type of the query.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder SetType(string type) {
			Contract.Requires(type != null);
			Contract.Requires(type == "select" || type == "update" || type == "insert" || type == "delete");

			_type = type;
			return this;
		}

		/// <summary>
		/// Set the table to be used in the query.
		/// </summary>
		/// <param name="table">The table to use in the query.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder SetTable(string table) {
			Contract.Requires(table != null);

			_table = table;
			return this;
		}

		/// <summary>
		/// Set the columns of the table that should be used in the query.
		/// </summary>
		/// <param name="columns">The columns of the table used in the query.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder SetColumns(string[] columns) {
			Contract.Requires(columns != null);
			Contract.Requires(columns.Length > 0);

			_columns.Clear();
			_columns.AddRange(columns);
			return this;
		}

		/// <summary>
		/// Set the values that should be inserted or updated in the query.
		/// This is only used when the type of query is either "insert" or
		/// "update".
		/// </summary>
		/// <param name="values">The values to be used for updating or inserting.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder SetValues(string[] values) {
			Contract.Requires(values != null);
			Contract.Requires(values.Length > 0);

			_values.Clear();
			_values.AddRange(values);

			return this;
		}

		/// <summary>
		/// Adding a WHERE condition to the query.
		/// </summary>
		/// <param name="condition">The condition to be used in the query.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder AddCondition(string condition) {
			Contract.Requires(condition != null);

			return AddCondition(condition, "and", condition);
		}

		/// <summary>
		/// Adding a WHERE condition to the query and specifying 
		/// the following logical operator (must be either "and" or "or").
		/// </summary>
		/// <param name="condition">The condition to be used in the query.</param>
		/// <param name="bind">The binding logical operator.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder AddCondition(string condition, string bind) {
			Contract.Requires(condition != null);
			Contract.Requires(bind != null);
			Contract.Requires(bind == "and" || bind == "or");

			return AddCondition(condition, bind, condition);
		}

		/// <summary>
		/// Adding a WHERE condition to the query, specifying 
		/// the following logical operator (must be either "and" or "or")
		/// and an index if the condition should be removed again.
		/// </summary>
		/// <param name="condition">The condition to be used in the query.</param>
		/// <param name="bind">The binding logical operator.</param>
		/// <param name="index">The identifying index of the condition.</param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder AddCondition(string condition, string bind, string index) {
			Contract.Requires(condition != null);
			Contract.Requires(bind != null);
			Contract.Requires(bind == "and" || bind == "or");
			Contract.Requires(index != null);

			_conditions.Add(index, new[] { condition, bind });
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns>The QueryBuilder instance for chaining.</returns>
		public QueryBuilder RemoveCondition(string index) {
			Contract.Requires(index != null);

			_conditions.Remove(index);
			return this;
		}

		public QueryBuilder SetLimit(int limit) {
			// Contract.Requires(limit > 0);
			_limit = limit;
			return this;
		}

		public QueryBuilder SetOffset(int offset) {
			// Contract.Requires(limit > -1);
			_offset = offset;
			return this;
		}

		public QueryBuilder AddOrder(string order) {
			// Contract.Requires(order != null);
			return AddOrder(order, "asc", order);
		}

		public QueryBuilder AddOrder(string order, string dir) {
			// Contract.Requires(order != null);
			// Contract.Requires(dir != null);
			return AddOrder(order, dir, order);
		}

		public QueryBuilder AddOrder(string order, string dir, string index) {
			// Contract.Requires(order != null);
			// Contract.Requires(dir != null);
			// Contract.Requires(index != null);
			_orders.Add(index, new[] { order, dir });
			return this;
		}

		public QueryBuilder RemoveOrder(string index) {
			// Contract.Requires(index != null);
			_orders.Remove(index);
			return this;
		}

		private string AssembleSelect() {
			var query = "SELECT";
			query += AssembleColumns();
			query += " FROM";
			query += AssembleTable();
			query += AssembleConditions();
			query += AssembleOrders();
			query += AssembleLimit();
			query += AssembleOffset();

			return query;
		}

		private string AssembleUpdate() {
			var query = "UPDATE";
			query += AssembleTable();
			query += " SET";

			var count = _columns.Count;
			for (var x = 0; x < count; x++) {
				var column = _columns[x];
				var value = _values[x];

				query += " `" + column + "` = '" + value + "'" + (x < count - 1 ? "," : "");
			}

			query += AssembleConditions();
			query += AssembleOrders();
			query += AssembleLimit();

			return query;
		}

		private string AssembleInsert() {
			var query = "INSERT INTO";
			query += AssembleTable();
			query += " (" + AssembleColumns() + " )";
			query += " VALUES";
			query += " (" + AssembleValues() + " )";
			query += AssembleConditions();

			return query;
		}

		private string AssembleDelete() {
			var query = "DELETE FROM";
			query += AssembleTable();
			query += AssembleConditions();
			query += AssembleOrders();
			query += AssembleLimit();

			return query;
		}

		private string AssembleTable() {
			return " `" + _table + "`";
		}

		private string AssembleColumns() {
			var query = " ";

			int x = 1, count = _columns.Count;
			foreach (string column in _columns) {
				query += "`" + column + "`" + (x < count ? ", " : "");

				x++;
			}

			return query;
		}

		private string AssembleValues() {
			var query = " ";

			int x = 1, count = _values.Count;
			foreach (string value in _values) {
				query += "'" + value + "'" + (x < count ? ", " : "");

				x++;
			}

			return query;
		}

		private string AssembleConditions() {
			var query = "";
			int x = 1, count = _conditions.Count;
			var enumerator = _conditions.GetEnumerator();

			if (enumerator != null && count > 0) {
				query += " WHERE";

				while (enumerator.MoveNext()) {
					var condition = ((string[])enumerator.Value)[0];
					var bind = ((string[])enumerator.Value)[1];

					query += " (" + condition + ")" + (x < count ? " " + bind.ToUpper() : "");

					x++;
				}
			}

			return query;
		}

		private string AssembleOrders() {
			var query = "";
			int x = 1, count = _orders.Count;
			var enumerator = _orders.GetEnumerator();

			if (enumerator != null && count > 0) {
				query += " ORDER BY";
				while (enumerator.MoveNext()) {
					var order = ((string[])enumerator.Value)[0];
					var dir = ((string[])enumerator.Value)[1];

					query += " `" + order + "` " + dir.ToUpper() + (x < count ? "," : "");

					x++;
				}
			}

			return query;
		}

		private string AssembleLimit() {
			return _limit > -1 ? " LIMIT " + _limit : "";
		}

		private string AssembleOffset() {
			return _offset > -1 ? " OFFSET " + _offset : "";
		}

		public string GetQuery() {
			return _query;
		}

		public int GetCount() {
			return _connector.GetCount();
		}

		public int GetCountTotal() {
			return _connector.GetCountTotal();
		}

		public string Assemble() {
			var query = "";

			switch (_type) {
				case "select":
					query = AssembleSelect();
					break;
				case "update":
					query = AssembleUpdate();
					break;
				case "insert":
					query = AssembleInsert();
					break;
				case "delete":
					query = AssembleDelete();
					break;
			}

			_query = query;

			Clear();

			return query;
		}

		public ArrayList ExecuteQuery() {
			return _connector.ExecuteQuery(Assemble());
		}

		public int ExecuteNoneQuery() {
			return _connector.ExecuteNoneQuery(Assemble());
		}
	}
}