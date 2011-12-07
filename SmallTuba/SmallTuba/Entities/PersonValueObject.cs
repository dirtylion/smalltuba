namespace SmallTuba.Entities {
	using SmallTuba.Entities.Abstracts;
	
	public class PersonValueObject : AbstractValueObject {
		public PersonValueObject() {
			Columns = Person.Columns;
		}
	}
}