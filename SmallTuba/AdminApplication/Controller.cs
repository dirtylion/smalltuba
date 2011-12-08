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

    using SmallTuba.Entities;
    using SmallTuba.PdfGenerator;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Controller
    {
        private Form1 form;
        public Controller()
        {
            form = new Form1();
            
            form.GenerateVoterList.Click += new EventHandler(this.OpenFileSaveDialogVoterList);
            form.GeneratePollingCards.Click += new EventHandler(this.OpenFileSaveDiaglogPollingCards);

            form.TableView.Rows.Add(new String[]{"hej", "med"});
            form.TableView.Rows.Add(new String[] { "hej2", "med2" });
            this.ShowGui();
        }

        public void OpenFileSaveDialogVoterList(Object sender, EventArgs e)
        {
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
 
            if(form.SaveFileDialog.ShowDialog()== DialogResult.OK)
            {
                this.SaveVoterList();
                
            }  
        }

        public void OpenFileSaveDiaglogPollingCards(Object sender, EventArgs e)
        {
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.SavePollingCards();

            }  
        }

        private void SaveVoterList()
        {
            String path = form.SaveFileDialog.FileName;
            String name = form.ElectionName.Text;
            String date = form.ElectionDate.Text;
            VoterList voterlist = new VoterList(50, name, date, "Bord 1");
            voterlist.AddVoter();
            voterlist.SaveToDisk(path);
        }

        private void SavePollingCards()
        {
            String path = form.SaveFileDialog.FileName;
            String name = form.ElectionName.Text;
            String date = form.ElectionDate.Text;
            PollingCards pollingCards = new PollingCards(name, date, "09.00 - 20.00");
            pollingCards.CreatePollingCard(new Person());
            pollingCards.SaveToDisk(path);
            

        }

        public void ShowGui()
        {
            
            Application.Run(form);
        }
    }
        
}
