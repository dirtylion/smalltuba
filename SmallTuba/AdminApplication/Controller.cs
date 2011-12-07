// -----------------------------------------------------------------------
// <copyright file="Controller1.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
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
        private Form1 form;
        public Controller()
        {
            form = new Form1();
            
            form.GeneratePollingCards.Click += new EventHandler(this.OpenDialog);
                 


            this.ShowGui();
        }

        public void OpenDialog(Object sender, EventArgs e)
        {
            
            form.SaveFileDialog.ShowDialog();
        }

        public void ShowGui()
        {
            
            Application.Run(form);
        }
    }
        
}
