namespace SmallTuba.Entities.Abstracts {
	using System.Collections;
	using SmallTuba.Database;

	// using System.Diagnostics.Contracts;
	
	public abstract class AbstractDataAccessObject {
		protected QueryBuilder QueryBuilder;
		public string Table;
		public string[] Columns;

		protected AbstractDataAccessObject() {
			QueryBuilder = new QueryBuilder();
		}
		
		public Hashtable Load(Hashtable parameters) {
			QueryBuilder.SetType("select");
			QueryBuilder.SetTable(Table);
			QueryBuilder.SetColumns(Columns);
			QueryBuilder.SetLimit(1);

			var enumerator = parameters.GetEnumerator();
			while (enumerator.MoveNext()) {
				QueryBuilder.AddCondition("`"+enumerator.Key+"` = '"+enumerator.Value+"'");
			}

			var results = QueryBuilder.ExecuteQuery();

			return results.Count > 0 ? (Hashtable) results[0] : new Hashtable();
		}
		
		public void Save(Hashtable values) {
			var _values = new string[Columns.Length];

			for (var i = 0; i < Columns.Length; i++) {
				if (values.ContainsKey(Columns[i])) {
					_values[i] = values[Columns[i]].ToString();
				}
			}
			
			QueryBuilder.SetTable(Table);
			QueryBuilder.SetColumns(Columns);
			QueryBuilder.SetValues(_values);
			QueryBuilder.SetLimit(1);

			if (values.ContainsKey("id") && values["id"].ToString() != "" && (int) values["id"] > 0) {
				QueryBuilder.SetType("update");
				QueryBuilder.AddCondition("`id` = '" + (int)values["id"] + "'");
			} else {
				QueryBuilder.SetType("insert");
			}

			QueryBuilder.ExecuteNoneQuery();
		}
		
		public void Delete(int id) {
			QueryBuilder.SetType("delete");
			QueryBuilder.SetTable(Table);
			QueryBuilder.AddCondition("`id` = '" + id + "'");
			QueryBuilder.SetLimit(1);

			QueryBuilder.ExecuteNoneQuery();
		}
	}
}
