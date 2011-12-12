// <copyright file="VoterServerFactory.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using SmallTuba.Entities;
using SmallTuba.Network.RPC;
using SmallTuba.Utility;

namespace SmallTuba.Network.RPC
{
    /// <summary>A factory for SmallTuba.Network.RPC.VoterServer instances</summary>
    public static partial class VoterServerFactory
    {
        /// <summary>A factory for SmallTuba.Network.RPC.VoterServer instances</summary>
        [PexFactoryMethod(typeof(VoterServer))]
        public static VoterServer Create()
        {
            int unixTime = (int)TimeConverter.ConvertToUnixTimestamp(DateTime.Now.ToUniversalTime());
            Person person1 = new Person()
            {
                Cpr = "1",
                FirstName = "TestCprFirst",
                DbId = 11,
                LastName = "TestCprLast",
                VotedPollingTable = "Table 1",
                VotedTime = unixTime,
                Voted = false,
                Exists = true
            };
            Person person2 = new Person()
            {
                Cpr = "2",
                FirstName = "TestIdFirst",
                DbId = 22,
                LastName = "TestIdFirst",
                VotedPollingTable = "Table 2",
                VotedTime = unixTime,
                Voted = true,
                Exists = true
            };
            Person emptyPerson = new Person();
            string[] tables = new string[] { "Table 1", "Table 2", "Table 3" };
    
            VoterServer voterServer = new VoterServer("Client");
            voterServer.SetUnregisterVoteRequest((name, person) => !person.Voted);
            voterServer.SetValidTableRequest(name => tables);
            voterServer.SetCprToPersonRequest((name, cpr) => cpr == "1" ? person1 : emptyPerson);
            voterServer.SetVoterIdToPersonRequest((name, id) => id == 2 ? person2 : emptyPerson);
            voterServer.SetRegisterVoteRequest((name, person) => !person.Voted);
            return voterServer;
            
            // TODO: Edit factory method of VoterServer
            // This method should be able to configure the object in all possible ways.
            // Add as many parameters as needed,
            // and assign their values to each field by using the API.
        }
    }
}
