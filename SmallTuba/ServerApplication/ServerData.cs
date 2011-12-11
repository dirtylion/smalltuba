namespace ServerApplication
{
	using System;
	using System.IO;

	using SmallTuba.Database;
	using SmallTuba.Entities;

	public class ServerData {

		public bool Import(string[] args) {

			string file = String.Join(" ", args);
			file = file.Substring(7);
			file = file.Trim();

			if (args.Length > 1 && file != "") {
				try {
					using (StreamReader reader = new StreamReader(file)) {
						Console.WriteLine("Importing from: "+file);

						string line;
						string[] row;
						PersonEntity personEntity;
						
						var i = 0;	
						while ((line = reader.ReadLine()) != null) {
							if (i > 0) {
								row = line.Split(';');

								personEntity = new PersonEntity {
									Firstname = row[0],
									Lastname = row[1],
									Cpr = row[2],
									VoterId = Convert.ToInt32(row[3])
								};

								Console.WriteLine("Imported: "+personEntity.VoterId);

								personEntity.Save();
							}
							
							i++;
						}
					}

					return true;
				} catch (Exception e) { }
			}
			
			return false;
		}

		public bool Clear(string[] args) {
			if (args.Length > 1 && args[1] != null && args[1] != "" && (args[1] == "person" || args[1] == "log")) {
				var queryBuilder = new QueryBuilder();
				queryBuilder.SetType("truncate");
				queryBuilder.SetTable(args[1]);

				queryBuilder.ExecuteNoneQuery();

				return true;
			}

			return false;
		}
	}
}
