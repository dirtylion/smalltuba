namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// </summary>
	public class PersonResource : AbstractResource {
		public List<Person> Build() {
			QueryBuilder.SetType("select");
			QueryBuilder.SetTable(Person.Table);
			QueryBuilder.SetColumns(Person.Columns);

			var results = QueryBuilder.ExecuteQuery();

			var entities = new List<Person>();

			foreach (var result in results) {
				entities.Add(new Person((Hashtable) result));
			}
			
			return entities;
		}
	}
}
