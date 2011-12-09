// -----------------------------------------------------------------------
// <copyright file="FileLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.Schema;

	using SmallTuba.Entities;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class FileLoader
	{
		public List<PollingVenue> GetPollingVenues(string path, ValidationEventHandler notifier)
		{
			return this.LoadVenues(XDocument.Load(path), notifier);		
		}

		private List<PollingVenue> LoadVenues(XDocument xDocument, ValidationEventHandler notifier)
		{
			if (!this.ValidateXmlFile(xDocument, notifier))
			{
				var pollingVenuesElements = from n in xDocument.Descendants("PollingVenue") select n;

				List<PollingVenue> pollingVenues = new List<PollingVenue>();
				foreach (var xElement in pollingVenuesElements)
				{
					Address pollingVenueAddress = new Address
					{
						Name = xElement.Element("Name").Value,
						Street = xElement.Element("Street").Value,
						City = xElement.Element("City").Value
					};

					Address municipalityAddress = new Address
					{
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

			return null;
		}


		private List<Person> LoadPersons(XElement xelement)
		{
			List<Person> persons = new List<Person>();
			var personElements = from n in xelement.Descendants("Voter") select n;

			foreach (var element in personElements)
			{
				Person person = new Person {
					FirstName = element.Element("FirstName").Value,
					LastName = element.Element("LastName").Value,
					Street = element.Element("Street").Value,
					City = element.Element("City").Value,
					Cpr = Convert.ToInt32(element.Element("CprNo").Value),
					PollingTable = Convert.ToInt32(element.Element("PollingTable").Value),
					VoterId = VoterIdGenerator.CreateVoterId()
				};

				persons.Add(person);
			}
			return persons;
		}

		private bool ValidateXmlFile(XDocument xDocument, ValidationEventHandler notifier)
		{
			XmlSchemaSet schemas = new XmlSchemaSet();
			schemas.Add("", XmlReader.Create("schema.xml"));
			bool error = false;
			xDocument.Validate(schemas, (o,e) =>
				{
					notifier(o, e);
					error = true;
				});
			return error;
		}
		
	}
}
