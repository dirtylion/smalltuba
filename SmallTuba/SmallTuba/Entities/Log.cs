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
		public static readonly string[] Columns = { "id", "client_id", "person_id", "action", "timestamp" };

		public Log() {
			ValueObject = new LogValueObject();
			DataAccessObject = new LogDataAccessObject();
		}

		public Log(Hashtable values) : this() {
			ValueObject.SetValues(values);
		}

		public string ClientId { 
			get { return (string) ValueObject["client_id"]; } 
			set { ValueObject["client_id"] = value; }
		}
		public string PersonId { 
			get { return (string) ValueObject["person_id"]; } 
			set { ValueObject["person_id"] = value; }
		}
		public string Action { 
			get { return (string) ValueObject["action"]; } 
			set { ValueObject["action"] = value; }
		}
		public string Timestamp { 
			get { return (string) ValueObject["timpestamp"]; } 
			set { ValueObject["timpestamp"] = value; }
		}
	}
}