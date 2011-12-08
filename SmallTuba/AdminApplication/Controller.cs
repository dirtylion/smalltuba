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

            form.GenerateVoterList.Click += new EventHandler(this.FolderBrowser);
            form.GeneratePollingCards.Click += new EventHandler(this.FileSaveDiaglogPollingCards);
            form.ImportData.Click += new EventHandler(this.FileOpenDialogImport);

            List<Address> a = new List<Address>();
            a.Add(new Address("skole1", "vej1", "by1"));
            a.Add(new Address("skole2", "vej2", "by2"));
            this.UpdateTable(a);

            this.ShowGui();
        }

        public void FileSaveDiaglogPollingCards(Object sender, EventArgs e)
        {
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.SavePollingCards();

            }  
        }

        public void FolderBrowser(Object sender, EventArgs e)
        {
            if (form.FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var a = new List<Person>();
                var person = new Person();
                person.PollingTable = "bord 1";
                person.Firstname = "Kåre";
                person.Lastname = "Sylow";
                person.Cpr = "010101-0101";
                person.Barcode = "123456789";

                var person2 = new Person();
                person2.PollingTable = "bord 1";
                person2.Firstname = "Kåre";
                person2.Lastname = "Sylow";
                person2.Cpr = "010101-0101";
                person2.Barcode = "123456789";
                a.Add(person);
                a.Add(person2);
                Console.WriteLine(a.Count);
                this.SaveVoterList(a);
            }
        }

        public void FileOpenDialogImport(Object sender, EventArgs e)
        {
            GetVenueFromTable();
            form.OpenFileDialog.Filter = "Vores evil format (*.xxx)|*.xxx";
            if (form.OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Load file
            }
        }

        public void UpdateTable(List<Address> pollingVenues)
        {
            form.TableView.DataSource = pollingVenues;
        }

        public void GetVenueFromTable()
        {
            Console.WriteLine(form.TableView.SelectedRows[0].Index);
        }

        private void SaveVoterList(List<Person> persons)
        {
            String path = form.FolderBrowserDialog.SelectedPath;
            String electionName = form.ElectionName.Text;
            String electionDate = form.ElectionDate.Text;

            Dictionary<string, VoterList> voterlists = this.CreateVoterListsForVenue(persons, electionName, electionDate);
            this.AddVotersToVoterlists(persons, voterlists);
            this.SaveVoterListsToDisk(path, voterlists);
        }

        private Dictionary<string, VoterList> CreateVoterListsForVenue(List<Person> persons, string electionName, string electionDate)
        {
            Dictionary<string, VoterList> voterlists = new Dictionary<string, VoterList>();
            foreach (var person in persons)
            {
                if (!voterlists.ContainsKey(person.PollingTable))
                {
                    voterlists.Add(person.PollingTable, new VoterList(50, electionName, electionDate, person.PollingTable));
                }
            }
            return voterlists;
        }

        private void AddVotersToVoterlists(List<Person> persons, Dictionary<string, VoterList> voterlists)
        {
            foreach (var person in persons)
            {
                voterlists[person.PollingTable].AddVoter(person);
            }
        }

        private void SaveVoterListsToDisk(string path, Dictionary<string, VoterList> voterlists)
        {
            foreach (var pollingTabel in voterlists.Keys)
            {
                voterlists[pollingTabel].SaveToDisk(path+ "\\"+pollingTabel +".pdf");
            }
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
