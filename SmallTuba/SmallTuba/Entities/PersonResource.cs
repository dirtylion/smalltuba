namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	public class PersonResource : AbstractResource<PersonEntity> {

		public PersonResource SetFirstname(string firstname) {
			Contract.Requires(firstname != null);
			QueryBuilder.AddCondition("`firstname` = '"+firstname+"'");

			return this;
		}

		public PersonResource SetLastname(string lastname) {
			Contract.Requires(lastname != null);
			QueryBuilder.AddCondition("`lastname` = '"+lastname+"'");

			return this;
		}

		// TODO SHOULD BE DELETED
		public PersonResource SetGender(int gender) {
			Contract.Requires(gender == 1 || gender == 2);
			QueryBuilder.AddCondition("`gender` = '"+gender+"'");

			return this;
		}

		public PersonResource SetCpr(int cpr) {
			Contract.Requires(cpr > 0);
			QueryBuilder.AddCondition("`cpr` = '"+cpr+"'");

			return this;
		}

		public PersonResource SetPollingVenue(string pollingVenue) {
			Contract.Requires(pollingVenue != null);
			QueryBuilder.AddCondition("`polling_venue` = '"+pollingVenue+"'");

			return this;
		}

		public PersonResource SetPollingTable(string pollingTable) {
			Contract.Requires(pollingTable != null);
			QueryBuilder.AddCondition("`polling_table` = '"+pollingTable+"'");

			return this;
		}

		public PersonResource SetVoterId(int voter_id) {
			Contract.Requires(voter_id > 0);
			QueryBuilder.AddCondition("`voter_id` = '"+voter_id+"'");

			return this;
		}

		public override List<PersonEntity> Build() {
			QueryBuilder.SetType("select");
			QueryBuilder.SetTable(PersonEntity.Table);
			QueryBuilder.SetColumns(PersonEntity.Columns);

			var results = QueryBuilder.ExecuteQuery();

			var entities = new List<PersonEntity>();

			foreach (var result in results) {
				entities.Add(new PersonEntity((Hashtable) result));
			}
			
			return entities;
		}
	}
}
