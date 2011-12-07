namespace SmallTuba.Entities.Abstracts
{

	using SmallTuba.Database;

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-01</version>
	/// <summary>
	/// </summary>
	public abstract class AbstractResource {
		protected QueryBuilder QueryBuilder;
		
		protected AbstractResource() {
			QueryBuilder = new QueryBuilder();
		}

		public void SetOrder(string order, string direction) {
			QueryBuilder.AddOrder(order, direction);
		}

		public void SetLimit(int limit) {
			QueryBuilder.SetLimit(limit);
		}

		public void SetOffset(int offset) {
			QueryBuilder.SetOffset(offset);
		}

		public int GetCount() {
			return QueryBuilder.GetCount();
		}

		public int GetCountTotal() {
			return QueryBuilder.GetCountTotal();
		}
	}
}