// <copyright file="VoterServerFactory.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using SmallTuba.Network.RPC;

namespace SmallTuba.Network.RPC
{
    /// <summary>A factory for SmallTuba.Network.RPC.VoterServer instances</summary>
    public static partial class VoterServerFactory
    {
        /// <summary>A factory for SmallTuba.Network.RPC.VoterServer instances</summary>
        [PexFactoryMethod(typeof(VoterServer))]
        public static VoterServer Create(
            string name_s,
            VoterServer.UnregisterVoteRequest function_unregisterVoteRequest,
            VoterServer.ValidTableRequest function_validTableRequest,
            VoterServer.CprToPersonRequest function_cprToPersonRequest,
            VoterServer.VoterIdToPersonRequest function_voterIdToPersonRequest,
            VoterServer.RegisterVoteRequest function_registerVoteRequest
        )
        {
            VoterServer voterServer = new VoterServer(name_s);
            voterServer.SetUnregisterVoteRequest(function_unregisterVoteRequest);
            voterServer.SetValidTableRequest(function_validTableRequest);
            voterServer.SetCprToPersonRequest(function_cprToPersonRequest);
            voterServer.SetVoterIdToPersonRequest(function_voterIdToPersonRequest);
            voterServer.SetRegisterVoteRequest(function_registerVoteRequest);
            return voterServer;

            // TODO: Edit factory method of VoterServer
            // This method should be able to configure the object in all possible ways.
            // Add as many parameters as needed,
            // and assign their values to each field by using the API.
        }
    }
}
