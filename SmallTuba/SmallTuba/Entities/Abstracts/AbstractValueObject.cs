namespace SmallTuba.Entities.Abstracts {
	using System.Collections;
	// using System.Diagnostics.Contracts;
	
	public abstract class AbstractValueObject {
		protected string[] Columns;
		private readonly Hashtable values;
		
		protected AbstractValueObject () {
			values = new Hashtable();
		}
		
		public object this[string key] {
			get { return values[key]; }
			set { values[key] = value; }
		}
		
		public void SetValues(Hashtable values) {
			var enumerator = values.GetEnumerator();
			
			while (enumerator.MoveNext()) {
				this[(string) enumerator.Key] = enumerator.Value;
			}
		}
		
		public Hashtable GetValues() {
			return values;
		}
	}
}