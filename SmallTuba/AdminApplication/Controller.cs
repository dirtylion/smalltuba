// -----------------------------------------------------------------------
// <copyright file="Controller1.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
{
    using System;
    using System.Collections.Generic;
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
            
            form.GenerateVoterList.Click += new EventHandler(this.FileSaveDialogVoterList);
            form.GeneratePollingCards.Click += new EventHandler(this.FileSaveDiaglogPollingCards);
            form.ImportData.Click += new EventHandler(this.FileOpenDialogImport);

            this.ShowGui();
        }

        public void FileSaveDialogVoterList(Object sender, EventArgs e)
        {
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
 
            if(form.SaveFileDialog.ShowDialog()== DialogResult.OK)
            {
                this.SaveVoterList();
                
            }  
        }

        public void FileSaveDiaglogPollingCards(Object sender, EventArgs e)
        {
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.SavePollingCards();

            }  
        }

        public void FileOpenDialogImport(Object sender, EventArgs e)
        {
            List<Address> a = new List<Address>();
            a.Add(new Address("skole1", "vej1", "by1"));
            a.Add(new Address("skole2", "vej2", "by2"));
            this.UpdateTable(a);
            form.OpenFileDialog.Filter = "Vores evil format (*.xxx)|*.xxx";
            if (form.OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Load file
            }
        }

        public void UpdateTable(List<Address> pollingVenues)
        {
            form.TableView.Rows.Clear();
            foreach (var pollingVenue in pollingVenues)
            {
                //form.TableView.Rows.Add(new String[]{pollingVenue.Name, pollingVenue.Road, pollingVenue.City});
                form.TableView.Rows.Add(pollingVenue);
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
