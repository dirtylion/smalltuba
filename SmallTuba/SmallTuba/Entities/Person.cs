namespace SmallTuba.Entities {
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// The Person class represents a real-life person, with
	/// relevant-to-the-system information associated such as
	/// the persons name and CPR number.
	/// </summary>
	public class Person : AbstractEntity {
		public static readonly string Table = "Person";
		public static readonly string[] Columns = { "id", "firstname", "lastname", "cpr", "barcode", "polling_venue", "polling_table" };

		private List<Log> logs;

		public Person() {
			ValueObject = new PersonValueObject();
			DataAccessObject = new PersonDataAccessObject();

			logs = null;
		}

		public Person(Hashtable values) : this() {
			Contract.Requires(values != null);

			ValueObject.SetValues(values);
		}

		public string Firstname { 
			get { return ValueObject["firstname"] != null ? (string) ValueObject["firstname"] : ""; } 
			set { ValueObject["firstname"] = value; }
		}
		public string Lastname { 
			get { return ValueObject["lastname"] != null ? (string) ValueObject["lastname"] : ""; } 
			set { ValueObject["lastname"] = value; }
		}
		public int Cpr { 
			get { return ValueObject["cpr"] != null ? (int) ValueObject["cpr"] : 0; } 
			set { ValueObject["cpr"] = value; }
		}
		public int Barcode { 
			get { return ValueObject["barcode"] != null ? (int) ValueObject["barcode"] : 0; } 
			set { ValueObject["barcode"] = value; }
		}
		public string PollingVenue { 
			get { return ValueObject["polling_venue"] != null ? (string) ValueObject["polling_venue"] : ""; } 
			set { ValueObject["polling_venue"] = value; }
		}
		public string PollingTable { 
			get { return ValueObject["polling_table"] != null ? (string) ValueObject["polling_table"] : ""; } 
			set { ValueObject["polling_table"] = value; }
		}
		public bool Voted {
			get { return GetMostRecentLog() != null && GetMostRecentLog().Action == "register"; }
		}

		public int VotedTime {
			get { return GetMostRecentLog() != null && GetMostRecentLog().Action == "register" ? GetMostRecentLog().Timestamp : 0; }
		}

		public List<Log> GetLogs() {
			if (Exists()) {
				var resource = new LogResource();
				resource.SetPerson(this);
				resource.SetOrder("timestamp", "desc");

				logs = resource.Build();
			}

			return logs;
		}

		public Log GetMostRecentLog() {
			return GetLogs() != null ? logs[0] : null;
		}

		public PersonState ToStateObject() {
			return new PersonState { 
				Id = Id, 
				Firstname = Firstname,
				Lastname = Lastname,
				Cpr = Cpr,
				Barcode = Barcode,
				PollingVenue = PollingVenue,
				PollingTable = PollingTable,
				Voted = Voted,
				VotedTime = VotedTime,
				Exists = Exists()
			};
		}
	}
}