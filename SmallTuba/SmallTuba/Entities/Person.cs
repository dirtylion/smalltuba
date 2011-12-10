﻿namespace SmallTuba.Entities {
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
	}
}
