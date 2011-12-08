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
	public class PersonEntity : AbstractEntity {
		public static readonly string Table = "Person";
		public static readonly string[] Columns = { "id", "firstname", "lastname", "cpr", "voter_id", "polling_venue", "polling_table" };

		private List<LogEntity> logs;

		public PersonEntity() {
			ValueObject = new PersonValueObject();
			DataAccessObject = new PersonDataAccessObject();

			logs = null;
		}

		public PersonEntity(Hashtable values) : this() {
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
		public int VoterId { 
			get { return ValueObject["voter_id"] != null ? (int) ValueObject["voter_id"] : 0; } 
			set { ValueObject["voter_id"] = value; }
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

		public string VotedPollingTable {
			get { return GetMostRecentLog() != null && GetMostRecentLog().Action == "register" ? GetMostRecentLog().PollingTable : ""; }
		}

		public List<LogEntity> GetLogs() {
			if (Exists()) {
				var resource = new LogResource();
				resource.SetPerson(this);
				resource.SetOrder("timestamp", "desc");

				logs = resource.Build();
			}

			return logs;
		}

		public LogEntity GetMostRecentLog() {
			return GetLogs() != null && GetLogs().Count > 0 ? logs[0] : null;
		}

		public Person ToObject() {
			return new Person {
			    DbId = DbId,
			    FirstName = Firstname,
			    LastName = Lastname,
			    Cpr = Cpr,
			    VoterId = VoterId,
			    PollingVenue = PollingVenue,
			    PollingTable = PollingTable,
			    Voted = Voted,
			    VotedTime = VotedTime,
				VotedPollingTable = VotedPollingTable,
			    Exists = Exists()
			};
		}

		public Address AddressTo {
			get { return new Address("Kåre", "ungarnsagde 2", "2300 Kbh s"); }
		}
		public Address AddressFrom {
			get { return new Address("Rådhuset","Rådhuspladsen","1050 Kbh k"); }
		}
		public Address AddressPollingVenue {
			get { return new Address("Skole what ever", "bliv klog vej", "2300 kbh"); }
		}
	}
}