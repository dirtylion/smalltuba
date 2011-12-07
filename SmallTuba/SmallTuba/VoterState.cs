// -----------------------------------------------------------------------
// <copyright file="VoterState.cs" company="">
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
    public class VoterState
    {
        private int id;

        private bool voted;

        private DateTime date;

        private string table;

        public VoterState(int id, bool voted, DateTime date, string table)
        {
            this.id = id;
            this.voted = voted;
            this.date = date;
            this.table = table;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public bool HasVoted 
        {
            get 
            {
                return voted;
            } 
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string Table
        {
            get
            {
                return table;
            }
        }

        public override string ToString()
        {
            return id.ToString() + "," + voted.ToString() + "," + date.ToString() + "," + table;
        }
    }
}
