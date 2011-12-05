using System;
using System.Windows.Forms;

namespace SmallTuba
{
    //Hej Henrik

	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            // Tal pænt
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());

			// TODO ORDER PIZZA
		}
	}
}
