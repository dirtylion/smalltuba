// -----------------------------------------------------------------------
// <copyright file="Address.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct Address
    {
        private String name, road, city;

        public Address(String name, String road, String city)
        {
            this.name = name;
            this.road = road;
            this.city = city;
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String Road
        {
            get
            {
                return road;
            }
        }

        public String City
        {
            get
            {
                return city;
            }
        }

         
    }
}
