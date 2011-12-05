// -----------------------------------------------------------------------
// <copyright file="Keyword.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.Message
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describing the action to be performed by the server
    /// </summary>
    public enum Keyword
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
        /// Are you a request about getting the state of a voter?
        /// </summary>
        HasVoted,
        
        /// <summary>
        /// Are you a request about registering a voter?
        /// </summary>
        RegisterVoter,

        /// <summary>
        /// Are you a request about unregistering a voter?
        /// </summary>
        UnregisterVoter,
    }
}
