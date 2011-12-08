namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	public class LogResource : AbstractResource<LogEntity> {

		public LogResource SetPerson(PersonEntity personEntity) {
			Contract.Requires(personEntity != null);
			Contract.Requires(personEntity.Exists());

			QueryBuilder.AddCondition("`person_id` = '"+personEntity.DbId+"'");

			return this;
		}

		public override List<LogEntity> Build() {
			QueryBuilder.SetType("select");
			QueryBuilder.SetTable(LogEntity.Table);
			QueryBuilder.SetColumns(LogEntity.Columns);

			var results = QueryBuilder.ExecuteQuery();

			var entities = new List<LogEntity>();

			foreach (var result in results) {
				entities.Add(new LogEntity((Hashtable) result));
			}
			
			return entities;
		}
	}
}
