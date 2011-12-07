namespace SmallTuba.Entities {
	using SmallTuba.Entities.Abstracts;

	public class PersonDataAccessObject : AbstractDataAccessObject {
		public PersonDataAccessObject() {
			Table = Person.Table;
			Columns = Person.Columns;
		}
	}
}
