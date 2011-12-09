using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmallTuba.Utility;
using SmallTuba.Entities;

namespace ClientApplication
{
    public class ClientLog
    {
        private Person voter;

        private string action;

        private DateTime time;

        public ClientLog(Person voter, string action)
        {
            this.voter = voter;
            this.action = action;
            this.time = DateTime.Now;
        }

        public Person Voter
        {
            get { return voter; }
        }

        public override string ToString()
        {
            return time.ToLocalTime().Hour + ":" + time.ToLocalTime().Minute + " " + voter.FirstName + " " + voter.LastName + " was " + action;
        }
    }
}
