using System;
using System.Windows.Forms;
namespace AdminApplication {
	using System.Runtime.InteropServices;

	using SmallTuba.Utility;

	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			if (Debug.ConsoleOutput) {
				Win32.AllocConsole();
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Controller controller = new Controller();
			controller.Run();
		}

		public class Win32 {
			[DllImport("kernel32.dll")]
			public static extern Boolean AllocConsole();
		}
	}
}
