// -----------------------------------------------------------------------
// <copyright file="Keyword.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.RPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describing the action to be performed by the server
    /// </summary>
    internal enum Keyword
    {
        /// <summary>
        /// Are you a request about getting a person from a cpr number?
        /// </summary>
        GetPersonFromCpr,
        
        /// <summary>
        /// Are you a request about getting a person from an ID?
        /// </summary>
        GetPersonFromId,
        
        /// <summary>
        /// Are you a request about registering a voter?
        /// </summary>
        RegisterVoter,

        /// <summary>
        /// Are you a request about unregistering a voter?
        /// </summary>
        UnregisterVoter,

        /// <summary>
        /// Are you a request about valid tables?
        /// </summary>
        ValidTables,

        /// <summary>
        /// Are you connected to a server?
        /// </summary>
        Ping
    }
}
