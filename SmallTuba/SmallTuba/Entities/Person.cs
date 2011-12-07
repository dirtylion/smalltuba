﻿namespace SmallTuba.Entities {
	using System.Collections;

	using SmallTuba.Entities.Abstracts;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// The Person class represents a real-life person, with
	/// relevant-to-the-system information associated such as
	/// the persons name and CPR number.
	/// </summary>
	public class Person : AbstractEntity {
		public static readonly string Table = "Person";
		public static readonly string[] Columns = { "id", "firstname", "lastname", "cpr", "barcode", "polling_venue", "polling_table" };

		public Person() {
			ValueObject = new PersonValueObject();
			DataAccessObject = new PersonDataAccessObject();
		}

		public Person(Hashtable values) : this() {
			ValueObject.SetValues(values);
		}

		public string Firstname { 
			get { return (string) ValueObject["firstname"]; } 
			set { ValueObject["firstname"] = value; }
		}
		public string Lastname { 
			get { return (string) ValueObject["lastname"]; } 
			set { ValueObject["lastname"] = value; }
		}
		public string Cpr { 
			get { return (string) ValueObject["cpr"]; } 
			set { ValueObject["cpr"] = value; }
		}
		public string Barcode { 
			get { return (string) ValueObject["barcode"]; } 
			set { ValueObject["barcode"] = value; }
		}
		public string PollingVenue { 
			get { return (string) ValueObject["polling_venue"]; } 
			set { ValueObject["polling_venue"] = value; }
		}
		public string PollingTable { 
			get { return (string) ValueObject["polling_table"]; } 
			set { ValueObject["polling_table"] = value; }
		}
	}
}