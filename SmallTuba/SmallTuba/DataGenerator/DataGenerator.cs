namespace SmallTuba.DataGenerator {
	using System.Xml;

	public class DataGenerator {
		private DataSource dataSource;

		public DataGenerator() {
			dataSource = new DataSource();
			dataSource.Load();
		}

		public int NumberOfMunicipalities { get; set; }
		public int NumberOfPollingVenues { get; set; }
		public int NumberOfVoters { get; set; }

		public string FileDestination { get; set; }

		public void Generate() {
			XmlDocument doc = new XmlDocument();

			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			dec.Encoding = "UTF-8";

			doc.AppendChild(dec);

			doc = GenerateMunicipalites(doc);

			doc.Save(FileDestination);
		}

		private XmlDocument GenerateMunicipalites(XmlDocument doc) {
			XmlElement municipalities = doc.CreateElement("Municipalities");

			for (var i = 0; i < NumberOfMunicipalities; i++) {
				XmlElement municipality = doc.CreateElement("Municipality");

				var muni = dataSource.GetMunicipality();

				XmlElement name = doc.CreateElement("Name");
				name.InnerText = muni[0];
				municipality.AppendChild(name);

				XmlElement street = doc.CreateElement("Street");
				street.InnerText = muni[1];
				municipality.AppendChild(street);

				XmlElement city = doc.CreateElement("City");
				city.InnerText = muni[2];
				municipality.AppendChild(city);

				municipality.AppendChild(GeneratePollingVenues(doc, muni));

				municipalities.AppendChild(municipality);
			}

			doc.AppendChild(municipalities);

			return doc;
		}

		private XmlElement GeneratePollingVenues(XmlDocument doc, string[] municipality) {
			XmlElement pollingVenues = doc.CreateElement("PollingVenues");

			for (var i = 0; i < NumberOfPollingVenues; i++) {
				XmlElement pollingVenue = doc.CreateElement("PollingVenue");

				var venue = dataSource.GetPollingVenue(municipality);
				
				XmlElement name = doc.CreateElement("Name");
				name.InnerText = venue[0];
				pollingVenue.AppendChild(name);

				XmlElement street = doc.CreateElement("Street");
				street.InnerText = venue[1];
				pollingVenue.AppendChild(street);

				XmlElement city = doc.CreateElement("City");
				city.InnerText = venue[2];
				pollingVenue.AppendChild(city);

				pollingVenue.AppendChild(GenerateVoters(doc, venue));

				pollingVenues.AppendChild(pollingVenue);
			}

			return pollingVenues;
		}

		private XmlElement GenerateVoters(XmlDocument doc, string[] venue) {
			XmlElement voters = doc.CreateElement("Voters");

			for (var i = 0; i < NumberOfVoters; i++) {
				XmlElement voter = doc.CreateElement("Voter");

				var person = dataSource.GetVoter(venue);
				
				XmlElement firstName = doc.CreateElement("FirstName");
				firstName.InnerText = person[0];
				voter.AppendChild(firstName);

				XmlElement lastName = doc.CreateElement("LastName");
				lastName.InnerText = person[1];
				voter.AppendChild(lastName);

				XmlElement street = doc.CreateElement("Street");
				street.InnerText = person[2];
				voter.AppendChild(street);

				XmlElement city = doc.CreateElement("City");
				city.InnerText = person[3];
				voter.AppendChild(city);

				XmlElement cprNo = doc.CreateElement("CprNo");
				cprNo.InnerText = person[4];
				voter.AppendChild(cprNo);

				XmlElement pollingTable = doc.CreateElement("PollingTable");
				pollingTable.InnerText = person[5];
				voter.AppendChild(pollingTable);

				voters.AppendChild(voter);
			}

			return voters;
		}
	}
}
