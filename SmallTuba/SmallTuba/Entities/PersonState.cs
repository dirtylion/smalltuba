// -----------------------------------------------------------------------
// <copyright file="Person.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class PersonState
    {
        public int ID { get; set; }

        public int Cpr { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Table { get; set; }

        public DateTime Time { get; set; }

        public bool Voted { get; set; }

        public override string ToString()
        {
            return ID.ToString() + "," + Cpr.ToString() + "," + FirstName + "," + LastName + ", " + Table + ", " + Time.ToString() + ", " + Voted.ToString();
        }
    }
}
