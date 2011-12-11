// -----------------------------------------------------------------------
// <copyright file="Unique_Voter_Id_Generator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Utility
{
    using System;

    /// <summary>
    /// Generates a unique id number
    /// </summary>
    public class VoterIdGenerator
    {
        private static int prevId;
        private static int id;

        public static int CreateVoterId()
        {
            id = TimeConverter.ConvertToUnixTimestamp(DateTime.Now);
            while (id <= prevId)
            {
                id++;
            }
            prevId = id;
            return id;
        }

    }
}
