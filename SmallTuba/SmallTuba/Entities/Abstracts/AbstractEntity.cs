namespace SmallTuba.Entities.Abstracts {
	using System.Collections;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// The Person class represents a real-life person, with
	/// relevant-to-the-system information associated such as
	/// the persons name and CPR number.
	/// </summary>
	public abstract class AbstractEntity {
		protected AbstractValueObject ValueObject;
		protected AbstractDataAccessObject DataAccessObject;

		public void Load(Hashtable parameters) {
			ValueObject.SetValues(DataAccessObject.Load(parameters));
		}

		public void Save() {
			DataAccessObject.Save(ValueObject.GetValues());
		}

		public void Delete() {
			DataAccessObject.Delete(Id);
		}

		public int Id { 
			get { return (int) ValueObject["id"]; } 
			set { ValueObject["id"] = value; }
		}
	}
}
