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
	public class LogEntity : AbstractEntity {
		public static readonly string Table = "log";
		public static readonly string[] Columns = { "id", "person_id", "action", "client", "polling_table", "timestamp" };

		public LogEntity() {
			ValueObject = new LogValueObject();
			DataAccessObject = new LogDataAccessObject();
		}

		public LogEntity(Hashtable values) : this() {
			ValueObject.SetValues(values);
		}

		public int PersonDbId { 
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