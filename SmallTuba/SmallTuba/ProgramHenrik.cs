using System;

namespace SmallTuba {
	using System.Runtime.InteropServices;

	using SmallTuba.Entities;

	static class ProgramHenrik
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Win32.AllocConsole();

			Console.WriteLine("start");

			var resource = new LogResource();
			var logs = resource.Build();

			foreach (var log in logs) {
				Console.WriteLine(log.Action);
			}

			Console.WriteLine(resource.GetCount());
			Console.WriteLine(resource.GetCountTotal());

			


			Console.WriteLine("end");
			Console.ReadKey();
		}
	}

	public class Win32
	{
		[DllImport("kernel32.dll")]
		public static extern Boolean AllocConsole();
	}
}
