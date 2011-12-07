namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// </summary>
	public class LogResource : AbstractResource {
		public List<Log> Build() {
			QueryBuilder.SetType("select");
			QueryBuilder.SetTable(Log.Table);
			QueryBuilder.SetColumns(Log.Columns);

			var results = QueryBuilder.ExecuteQuery();

			var entities = new List<Log>();

			foreach (var result in results) {
				entities.Add(new Log((Hashtable) result));
			}
			
			return entities;
		}
	}
}
