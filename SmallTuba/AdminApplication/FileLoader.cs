// -----------------------------------------------------------------------
// <copyright file="FileLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using SmallTuba.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileLoader
    {
        public List<PollingVenue> GetPollingVenues(string path)
        {
            return this.LoadVenues(XDocument.Load(path));
        }

        private List<PollingVenue> LoadVenues(XDocument xDocument)
        {
            var pollingVenuesElements = from n in xDocument.Descendants("PollingVenue") select n;

            List<PollingVenue> pollingVenues = new List<PollingVenue>();
            foreach (var xElement in pollingVenuesElements)
            {
                Address pollingVenueAddress = new Address{
                    Name = xElement.Element("Name").Value, 
                    Street = xElement.Element("Street").Value, 
                    City = xElement.Element("City").Value
                };

                Address municipalityAddress = new Address{
                    Name = xElement.Parent.Parent.Element("Name").Value,
                    Street = xElement.Parent.Parent.Element("Street").Value,
                    City = xElement.Parent.Parent.Element("City").Value
                };

                PollingVenue pollingVenue = new PollingVenue
                {
                    Persons = this.LoadPersons(xElement),
                    PollingVenueAddress = pollingVenueAddress,
                    MunicipalityAddress = municipalityAddress
                };

                pollingVenues.Add(pollingVenue);
             }
            return pollingVenues;
        }


        private List<Person> LoadPersons(XElement xelement)
        {
            List<Person> persons = new List<Person>();
            var personElements = from n in xelement.Descendants("Voter") select n;

            foreach (var element in personElements)
            {
                Person person = new Person {
                    Firstname = element.Element("FirstName").Value,
                    Lastname = element.Element("LastName").Value,
                    Street = element.Element("Street").Value,
                    City = element.Element("City").Value,
                    Cpr = Convert.ToInt32(element.Element("CprNo").Value),
                    PollingTable = element.Element("PollingTable").Value
                };

                persons.Add(person);
            }
            return persons;
        } 
        
    }
}
