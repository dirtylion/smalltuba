namespace SmallTuba.Entities {
	using System;
	using System.Collections;
	using System.Collections.Generic;

	using SmallTuba.Utility;

	/// <author>Christian Ollson (chro@itu.dk)</author>
	/// <version>2011-12-07</version>
	/// <summary>
	/// A shallow copy reflecting properties of the real Person entity.
	/// </summary>
	[Serializable]
	public class Person {
		public int DbId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public int Cpr { get; set; }
		public int VoterId { get; set; }
		public string PollingVenue { get; set; }
		public string PollingTable { get; set; }
		public bool Voted { get; set; }
		public int VotedTime { get; set; }
		public string VotedPollingTable { get; set; }
		public bool Exists { get; set; }

		public override string ToString() {
			return DbId + "," + Cpr + "," + FirstName + "," + LastName + ", " + PollingTable + ", " + TimeConverter.ConvertFromUnixTimestamp(VotedTime) + ", " + Voted;
		}

        public static IComparer<Person> CprSort()
        {
            return (IComparer<Person>) new SortPersonsCpr();
        }

	    private class SortPersonsCpr : IComparer<Person>
        {
            public int Compare(Person a, Person b)
            {
                return a.Cpr.CompareTo(b.Cpr);
            }
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.DbId == DbId && Equals(other.FirstName, FirstName) && Equals(other.LastName, LastName) && Equals(other.Street, Street) && Equals(other.City, City) && other.Cpr == Cpr && other.VoterId == VoterId && Equals(other.PollingVenue, PollingVenue) && Equals(other.PollingTable, PollingTable) && other.Voted.Equals(Voted) && other.VotedTime == VotedTime && Equals(other.VotedPollingTable, VotedPollingTable) && other.Exists.Equals(Exists);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Person)) return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = DbId;
                result = (result * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                result = (result * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                result = (result * 397) ^ (Street != null ? Street.GetHashCode() : 0);
                result = (result * 397) ^ (City != null ? City.GetHashCode() : 0);
                result = (result * 397) ^ Cpr;
                result = (result * 397) ^ VoterId;
                result = (result * 397) ^ (PollingVenue != null ? PollingVenue.GetHashCode() : 0);
                result = (result * 397) ^ (PollingTable != null ? PollingTable.GetHashCode() : 0);
                result = (result * 397) ^ Voted.GetHashCode();
                result = (result * 397) ^ VotedTime;
                result = (result * 397) ^ (VotedPollingTable != null ? VotedPollingTable.GetHashCode() : 0);
                result = (result * 397) ^ Exists.GetHashCode();
                return result;
            }
        }
	}
}
