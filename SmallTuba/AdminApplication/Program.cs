using System;
using System.Windows.Forms;

namespace AdminApplication
{
    static class Program
    {
        /// <author>Kåre Sylow Pedersen (ksyl@itu.dk)</author>
        /// <version>2011-12-12</version>
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(){
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controller controller = new Controller();
            controller.Run();
        }
    }
}
