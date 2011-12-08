using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AdminApplication
{
    using System.Runtime.InteropServices;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Win32.AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Controller();
            Console.ReadKey();
        }

        public class Win32
        {
            [DllImport("kernel32.dll")]
            public static extern Boolean AllocConsole();
        }
    }
}
