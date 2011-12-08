using System;
using System.Windows.Forms;
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
                voterServer.SetCprToPersonRequest(cpr => new PersonState(){Cpr = cpr, FirstName = "Ole", ID = 42, LastName = "Henriksen", Table = "2", Time = DateTime.Now, Voted = false});
                voterServer.SetIdToPersonRequest(id => new PersonState() { Cpr = 42, FirstName = "Kim", ID = id, LastName = "Larsen", Table = "3", Time = DateTime.Now, Voted = true });
                voterServer.SetRegisterVoteRequest(person => !person.Voted);
                voterServer.SetUnregisterVoteRequest(person => !person.Voted);
                voterServer.SetValidTableRequest(() => new string[]{"Table 1", "Table 2", "Table 3"});
                voterServer.ListenForCalls(0);
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
                bool b = voterClient.RegisterVoter(new PersonState() { Cpr = 42, FirstName = "Ole", ID = 442, LastName = "Henriksen", Table = "2", Time = DateTime.Now, Voted = false });
                Console.Out.WriteLine("Register: " + b);
                Console.Out.WriteLine("4");
                bool bb = voterClient.UnregisterVoter(new PersonState() { Cpr = 43, FirstName = "Kim", ID = 443, LastName = "Larsen", Table = "3", Time = DateTime.Now, Voted = true });
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
