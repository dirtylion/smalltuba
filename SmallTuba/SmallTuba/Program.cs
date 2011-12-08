using System;
using SmallTuba.Entities;
using SmallTuba.Network.Voter;
using SmallTuba.Utility;

namespace SmallTuba
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// Tal pænt
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
			// TODO ORDER PIZZA

			int server = int.Parse(args[0]);
			if (server == 0)
			{
				Console.Out.WriteLine(System.Net.Dns.GetHostName() + " = name");
				VoterNetworkServer voterServer = new VoterNetworkServer(System.Net.Dns.GetHostName());
			    DateTime time = DateTime.Now;
                int unix = (int)TimeConverter.ConvertToUnixTimestamp(time.ToUniversalTime());
				voterServer.SetCprToPersonRequest(cpr => new Person(){Cpr = cpr, FirstName = "Ole", DbId = 42, LastName = "Henriksen", VotedPollingTable = "2", VotedTime = unix, Voted = false, Exists = true});
				voterServer.SetVoterIdToPersonRequest(id => new Person() { Cpr = 42, FirstName = "Kim", DbId = id, LastName = "Larsen", VotedPollingTable = "3", VotedTime = unix, Voted = true, Exists = false});
				voterServer.SetRegisterVoteRequest(person => person);
				voterServer.SetUnregisterVoteRequest(person => person);
				voterServer.SetValidTableRequest(() => new string[]{"Table 1", "Table 2", "Table 3"});
				voterServer.ListenForCalls(0);
			}
			else if (server == 1)
			{
				VoterNetworkClient voterClient = new VoterNetworkClient(System.Net.Dns.GetHostName());
				Console.Out.WriteLine("Connecected to server: " + voterClient.Connected());
				Console.Out.WriteLine(System.Net.Dns.GetHostName() + " = name");
				Console.Out.WriteLine("1");
				Person person = voterClient.GetPersonFromCpr(1);
				Console.Out.WriteLine("Person: " + person);
				Console.Out.WriteLine("2");
				person = voterClient.GetPersonFromId(1);
				Console.Out.WriteLine("Person: " + person);
				Console.Out.WriteLine("3");
				Person pp = voterClient.RegisterVoter(new Person() { Cpr = 42, FirstName = "Ole", DbId = 442, LastName = "Henriksen", PollingTable = "2", VotedTime = 0, Voted = false });
				Console.Out.WriteLine("Register: " + pp);
				Console.Out.WriteLine("4");
				Person ppp = voterClient.UnregisterVoter(new Person() { Cpr = 43, FirstName = "Kim", DbId = 443, LastName = "Larsen", PollingTable = "3", VotedTime = 0, Voted = true });
				Console.Out.WriteLine("Unregister: " + ppp);
				Console.Out.WriteLine("5");
				string[] arr = voterClient.ValidTables();
				string result = "";
				foreach (var temp in arr)
				{
					result += temp + ", ";
				}
				Console.Out.WriteLine("ValidTables: " + result);
			}

		}
	}
}
