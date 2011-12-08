namespace SmallTuba.Entities {
	using System.Collections;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// The Log class contains information on commands
	/// executed by a client interacting with a person
	/// entity in the system.
	/// object.
	/// </summary>
	public class Log : AbstractEntity {
		public static readonly string Table = "Log";
		public static readonly string[] Columns = { "id", "person_id", "action", "client", "polling_table", "timestamp" };

		public Log() {
			ValueObject = new LogValueObject();
			DataAccessObject = new LogDataAccessObject();
		}

		public Log(Hashtable values) : this() {
			ValueObject.SetValues(values);
		}

		public int PersonId { 
			get { return ValueObject["person_id"] != null ? (int) ValueObject["person_id"] : 0; } 
			set { ValueObject["person_id"] = value; }
		}
		public string Action { 
			get { return ValueObject["action"] != null ? (string) ValueObject["action"] : ""; } 
			set { ValueObject["action"] = value; }
		}
		public string Client { 
			get { return ValueObject["client"] != null ? (string) ValueObject["client"] : ""; } 
			set { ValueObject["client"] = value; }
		}
		public string PollingTable { 
			get { return ValueObject["polling_table"] != null ? (string) ValueObject["polling_table"] : ""; } 
			set { ValueObject["polling_table"] = value; }
		}
		public int Timestamp { 
			get { return ValueObject["timestamp"] != null ? (int) ValueObject["timestamp"] : 0; } 
			set { ValueObject["timestamp"] = value; }
		}
	}
}