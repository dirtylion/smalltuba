namespace SmallTuba.Entities {
	using System;

	/// <author>Christian Ollson (chro@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// A shallow copy reflecting properties of the real Person entity.
	/// </summary>
	[Serializable]
	public class PersonState {
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public int Cpr { get; set; }
		public int Barcode { get; set; }
		public string PollingVenue { get; set; }
		public string PollingTable { get; set; }
		public bool Voted { get; set; }
		public int VotedTime { get; set; }
		public string VotedPollingTable { get; set; }

		public bool Exists { get; set; }

		public override string ToString() {
			return Id.ToString() + "," + Cpr.ToString() + "," + Firstname + "," + Lastname + ", " + PollingTable + ", " + VotedTime.ToString() + ", " + Voted.ToString();
		}
	}
}
