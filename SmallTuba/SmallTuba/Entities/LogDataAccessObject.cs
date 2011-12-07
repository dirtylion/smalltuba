namespace SmallTuba.Entities {
	using SmallTuba.Entities.Abstracts;

	public class LogDataAccessObject : AbstractDataAccessObject {
		public LogDataAccessObject() {
			Table = Log.Table;
			Columns = Log.Columns;
		}
	}
}
