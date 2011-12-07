namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	public class PersonResource : AbstractResource<Person> {

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

		public PersonResource SetBarcode(int barcode) {
			Contract.Requires(barcode > 0);
			QueryBuilder.AddCondition("`barcode` = '"+barcode+"'");

			return this;
		}

		public override List<Person> Build() {
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
