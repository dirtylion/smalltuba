using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClientApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Win32.AllocConsole();

            Controller controller = new Controller();
            controller.Run();
        }
    }

    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
    }
}
