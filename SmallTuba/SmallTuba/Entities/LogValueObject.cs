namespace SmallTuba.Entities {
	using SmallTuba.Entities.Abstracts;
	
	public class LogValueObject : AbstractValueObject {
		public LogValueObject() {
			Columns = Log.Columns;
		}
	}
}