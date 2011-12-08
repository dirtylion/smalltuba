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

        public ClientLog(Person voter, string action)
        {
            this.voter = voter;
            this.action = action;
        }

        public Person Voter
        {
            get { return voter; }
        }

        public override string ToString()
        {
            DateTime time = TimeConverter.ConvertFromUnixTimestamp(voter.VotedTime);
            return time.ToLocalTime().Hour + ":" + time.ToLocalTime().Minute + " " + voter.FirstName + " " + voter.LastName + " was " + action;
        }
    }
}
