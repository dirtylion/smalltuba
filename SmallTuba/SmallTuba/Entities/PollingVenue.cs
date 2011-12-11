// -----------------------------------------------------------------------
// <copyright file="PollingVenue.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SmallTuba.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PollingVenue
    {
        public List<Person> Persons { get; set; }
        public Address PollingVenueAddress { get; set; }
        public Address MunicipalityAddress { get; set; }

    }

}
