using System;
using System.Windows.Forms;
using SmallTuba.Network.Voter;

namespace SmallTuba
{
    //Hej Henrik

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
                voterServer.SetCprToPersonRequest(cpr => new Person(42, cpr, "Hans", false));
                voterServer.SetIdToPersonRequest(id => new Person(id, 42, "Børge", true));
                voterServer.SetHasVotedRequest(id => new VoterState(id, true, DateTime.Now, "Bord 42"));
                voterServer.SetRegisterVoteRequest(voterState => voterState.HasVoted);
                voterServer.SetUnregisterVoteRequest(id => id < 10);
                voterServer.ListenForCalls(5000);
            }
            else if (server == 1)
            {
                Console.Out.WriteLine(System.Net.Dns.GetHostName() + " = name");
                Console.Out.WriteLine("1");
                VoterNetworkClient voterClient = new VoterNetworkClient(System.Net.Dns.GetHostName());
                Person person = voterClient.GetPersonFromCpr(1);
                Console.Out.WriteLine("Person: " + person);
                Console.Out.WriteLine("2");
                person = voterClient.GetPersonFromId(1);
                Console.Out.WriteLine("Person: " + person);
                Console.Out.WriteLine("3");
                VoterState state = voterClient.HasVoted(1);
                Console.Out.WriteLine("Voterstate: " + state);
                Console.Out.WriteLine("4");
                bool b = voterClient.RegisterVoter(new VoterState(42, true, DateTime.Now, "Bord 42"));
                Console.Out.WriteLine("Register: " + b);
                Console.Out.WriteLine("5");
                bool bb = voterClient.UnregisterVoter(42);
                Console.Out.WriteLine("Unregister: " + bb);
            }

		}
	}
}
