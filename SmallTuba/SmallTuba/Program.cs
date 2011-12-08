using System;
using SmallTuba.Entities;
using SmallTuba.Network.Voter;

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
				voterServer.SetCprToPersonRequest(cpr => new PersonState(){Cpr = cpr, Firstname = "Ole", Id = 42, Lastname = "Henriksen", PollingTable = "2", VotedTime = 0, Voted = false});
				voterServer.SetBarcodeToPersonRequest(id => new PersonState() { Cpr = 42, Firstname = "Kim", Id = id, Lastname = "Larsen", PollingTable = "3", VotedTime = 0, Voted = true });
				voterServer.SetRegisterVoteRequest(person => !person.Voted);
				voterServer.SetUnregisterVoteRequest(person => !person.Voted);
				voterServer.SetValidTableRequest(() => new string[]{"1", "2", "3"});
				voterServer.ListenForCalls(5000);
			}
			else if (server == 1)
			{
				VoterNetworkClient voterClient = new VoterNetworkClient(System.Net.Dns.GetHostName());
				Console.Out.WriteLine("Connecected to server: " + voterClient.Connected());
				Console.Out.WriteLine(System.Net.Dns.GetHostName() + " = name");
				Console.Out.WriteLine("1");
				PersonState person = voterClient.GetPersonFromCpr(1);
				Console.Out.WriteLine("Person: " + person);
				Console.Out.WriteLine("2");
				person = voterClient.GetPersonFromId(1);
				Console.Out.WriteLine("Person: " + person);
				Console.Out.WriteLine("3");
				bool b = voterClient.RegisterVoter(new PersonState() { Cpr = 42, Firstname = "Ole", Id = 442, Lastname = "Henriksen", PollingTable = "2", VotedTime = 0, Voted = false });
				Console.Out.WriteLine("Register: " + b);
				Console.Out.WriteLine("4");
				bool bb = voterClient.UnregisterVoter(new PersonState() { Cpr = 43, Firstname = "Kim", Id = 443, Lastname = "Larsen", PollingTable = "3", VotedTime = 0, Voted = true });
				Console.Out.WriteLine("Unregister: " + bb);
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
