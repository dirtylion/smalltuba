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
        private PersonState voter;

        private string action;

        public ClientLog(PersonState voter, string action)
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
            return TimeConverter.ConvertFromUnixTimestamp(voter.VotedTime).ToLocalTime().Hour + ":" + TimeConverter.ConvertFromUnixTimestamp(voter.VotedTime).ToLocalTime().Minute + " " + voter.Firstname + " " +
                   voter.Lastname + " was " + action;
        }
    }
}
