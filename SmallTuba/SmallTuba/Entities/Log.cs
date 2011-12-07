namespace SmallTuba.Entities {
	using System.Collections;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// The Person class represents a real-life person, with
	/// relevant-to-the-system information associated such as
	/// the persons name and CPR number.
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