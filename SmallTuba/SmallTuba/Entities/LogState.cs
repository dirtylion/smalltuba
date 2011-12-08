using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallTuba.Entities
{
    public class LogState
    {
        private PersonState voter;

        private string action;

        public LogState(PersonState voter, string action)
        {
            this.voter = voter;
            this.action = action;
        }

        public PersonState Voter
        {
            get { return voter; }
        }

        public override string ToString()
        {
            return voter.Time.ToLocalTime().Hour + ":" + voter.Time.ToLocalTime().Minute + " " + voter.FirstName + " " +
                   voter.LastName + " was " + action;
        }
    }
}
