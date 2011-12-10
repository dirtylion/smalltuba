namespace SmallTuba.DataGenerator {
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class DataSource {
		private Random random;

		public DataSource() {
			Console.WriteLine("DataSource");

			random = new Random();

			FirstNamesMen = new List<string>();
			FirstNamesWomen = new List<string>();
			LastNames = new List<string>();
			Municipalities = new List<string>();
			PollingVenues = new List<string>();
			Streets = new List<string>();
		}

		private List<string> FirstNamesMen { get; set; }
		private List<string> FirstNamesWomen { get; set; }
		private List<string> LastNames { get; set; }
		private List<string> Municipalities { get; set; }
		private List<string> PollingVenues { get; set; }
		private List<string> Streets { get; set; }

		public int RandomNumber(int limit) {
			return RandomNumber(1, limit);
		}

		public int RandomNumber(int offset, int limit) {
			return random.Next(offset > -1 ? offset : 0, limit);
		}

		public string[] GetMunicipality() {
			var index = RandomNumber(Municipalities.Count);
			var muni = Municipalities[index];

			var munis = muni.Split(' ');
			var name = munis[0]+" "+munis[1];

			var street = GenerateStreet();
			var city = (munis.Length == 3) ? munis[2]+" "+munis[0] : munis[0];

			return new[] { name, street, city };
		}

		public string[] GetPollingVenue(string[] municipality) {
			var index = RandomNumber(PollingVenues.Count);

			var venue = PollingVenues[index];
			var street = GenerateStreet();
			var city = municipality[2];

			return new[] { venue, street, city };
		}

		public string[] GetVoter(string[] venue) {
			var gender = (RandomNumber(0, 2) % 2 == 1);
			var index = RandomNumber(LastNames.Count);

			var firstName = "";
			var lastName = LastNames[index];
			var cprNo = GenerateBirthday();;
			var street = GenerateStreet();
			var city = venue[2];
			var pollingTable = RandomNumber(20)+"";

			if (gender) {
				index = RandomNumber(FirstNamesMen.Count);
				firstName = FirstNamesMen[index];
				cprNo += GenerateCprMen();
			} else {
				index = RandomNumber(FirstNamesWomen.Count);
				firstName = FirstNamesWomen[index];
				cprNo += GenerateCprWomen();
			}
			
			return new[] { firstName, lastName, street, city, cprNo, pollingTable };
		}

		private string GenerateBirthday() {
			return string.Format("{0:d2}", RandomNumber(1, 30))+
				string.Format("{0:d2}", RandomNumber(1, 12)) + 
				string.Format("{0:d2}", RandomNumber(11, 93));
		}

		private string GenerateCprMen() {
			var cpr = RandomNumber(1858, 2057);

			if (cpr % 2 == 0) {
				cpr++;
			}

			return cpr+"";
		}

		private string GenerateCprWomen() {
			var cpr = RandomNumber(1858, 2057);

			if (cpr % 2 == 1) {
				cpr++;
			}

			return cpr+"";
		}

		private string GenerateStreet() {
			var index = RandomNumber(Streets.Count);
			var street = Streets[index];

			street += " "+RandomNumber(200);

			return street;
		}

		public void Load() {
			LoadFirstNamesMen();
			LoadFirstNamesWomen();
			LoadLastNames();
			LoadMunicipalities();
			LoadPollingVenues();
			LoadStreets();
		}

		private void LoadFirstNamesMen() {
			var sr = new StreamReader("../../../data/firstnames-men.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				FirstNamesMen.Add(line);
			}
		}

		private void LoadFirstNamesWomen() {
			var sr = new StreamReader("../../../data/firstnames-women.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				FirstNamesWomen.Add(line);
			}
		}
		private void LoadLastNames() {
			var sr = new StreamReader("../../../data/lastnames.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				LastNames.Add(line);
			}
		}
		private void LoadMunicipalities() {
			var sr = new StreamReader("../../../data/municipalities.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				Municipalities.Add(line);
			}
		}
		private void LoadPollingVenues() {
			var sr = new StreamReader("../../../data/polling-venues.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				PollingVenues.Add(line);
			}
		}
		private void LoadStreets() {
			var sr = new StreamReader("../../../data/streets.txt");

			string line;
			while ((line = sr.ReadLine()) != null) {
				Streets.Add(line);
			}
		}
	}
}
