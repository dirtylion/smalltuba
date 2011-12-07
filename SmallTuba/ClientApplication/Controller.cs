// -----------------------------------------------------------------------
// <copyright file="Controller.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ClientApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
   using System.Windows.Forms;
 
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Controller
    {
        private WelcomeForm welcomeForm;

        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.welcomeForm = new WelcomeForm();
            this.welcomeForm.Load += new EventHandler(testStart);
            this.welcomeForm.RefreshButton.Click += new EventHandler(testRefresh);
            this.welcomeForm.OKButton.Click += new EventHandler(testOK);
            string[] arr = new string[] { "1", "2", "3" };
            this.welcomeForm.dropdown.Items.AddRange(arr);
            Application.Run(this.welcomeForm);
        }

        private void testRefresh(object o, EventArgs e)
        {
            Console.Out.WriteLine("Refreshed pressed");
        }

        private void testStart(object o, EventArgs e)
        {
            Console.Out.WriteLine("Start");
        }

        private void testOK(object o, EventArgs e)
        {
            Console.Out.WriteLine(this.welcomeForm.dropdown.SelectedItem);
        }
    }
}
