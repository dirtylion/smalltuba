// -----------------------------------------------------------------------
// <copyright file="Person.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Person
    {
        private int id;

        private int cpr;

        private string name;

        private bool voted;

        public Person(int id, int cpr, string name, bool voted)
        {
            this.id = id;
            this.cpr = cpr;
            this.name = name;
            this.voted = voted;
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }

        public int Cpr
        {
            get
            {
                return this.cpr;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public bool Voted
        {
            get
            {
                return this.voted;
            }
        }

        public override string ToString()
        {
            return id.ToString() + "," + cpr.ToString() + "," + name + "," + voted.ToString();
        }
    }
}
