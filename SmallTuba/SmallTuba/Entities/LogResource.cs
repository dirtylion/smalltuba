namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	public class LogResource : AbstractResource<Log> {

		public LogResource SetPerson(Person person) {
			Contract.Requires(person != null);
			Contract.Requires(person.Exists());

			QueryBuilder.AddCondition("`person_id` = '"+person.Id+"'");

			return this;
		}

		public override List<Log> Build() {
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
