// -----------------------------------------------------------------------
// <copyright file="PollingVenue.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// The class represent a real polling venue.
    /// The class contains all the voters, the address of the polling venue
    /// and the addresse of the municipality the polling venue is located in 
    /// </summary>
    public class PollingVenue
    {
        public List<Person> Persons { get; set; }
        public Address PollingVenueAddress { get; set; }
        public Address MunicipalityAddress { get; set; }

    }

}
