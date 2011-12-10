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
    using System.Windows.Forms;
    using System.Xml.Schema;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Controller
    {
        private List<PollingVenue> pollingVenues;
        private Form1 form;
        private FileLoader fileLoader;
        private FileSaver fileSaver;
        public event PollingVenuesChanged Changed;

        public delegate void PollingVenuesChanged();

        public Controller()
        {
            form = new Form1();
            fileLoader = new FileLoader();
            fileSaver = new FileSaver();
            this.InitializeEventSubscribers();
        }

        private void InitializeEventSubscribers()
        {
            form.GenerateVoterList.Click += this.FolderBrowserVoterLists;
            form.GeneratePollingCards.Click += this.FileSaveDiaglogPollingCards;
            form.ImportData.Click += this.FileOpenDialogImport;
            form.ExportData.Click += this.FileSaveDialogVotersExport;
            Changed += this.UpdateTable;
        }

        private void SetPollingVenues(string path)
        {
            pollingVenues = fileLoader.GetPollingVenues(path, this.ErrorLoadFileDialog);
            if (pollingVenues != null)
            {
                this.UpdateTable();
                Changed.Invoke();
            }     
        }

        private void UpdateTable()
        {
            var addresses = from n in pollingVenues select n.PollingVenueAddress;
            BindingSource bs = new BindingSource();
            bs.DataSource = addresses;
            form.TableView.DataSource = bs;
        }

        private void FileSaveDiaglogPollingCards(Object sender, EventArgs e)
        {
            if (this.GetSelectedPollingVenue() == null)
            {
                MessageBox.Show("No polling venue is selected", "Error", MessageBoxButtons.OK);
                return;
            }

            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileSaver.SavePollingCards(this.GetSelectedPollingVenue(), this.SelectedFilePath(), this.ElectionName(), this.ElectionDate());
            }  
        }

        private void FileSaveDialogVotersExport(Object sender, EventArgs e)
        {
            if (this.GetSelectedPollingVenue() == null)
            {
                MessageBox.Show("No polling venue is selected", "Error", MessageBoxButtons.OK);
                return;
            }

            form.SaveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileSaver.SaveVoters(this.GetSelectedPollingVenue().Persons, this.SelectedFilePath());
            }
        }

        private void FolderBrowserVoterLists(Object sender, EventArgs e)
        {
            if (this.GetSelectedPollingVenue() == null)
            {
                MessageBox.Show("No polling venue is selected", "Error", MessageBoxButtons.OK);
                return;
            }

            if (form.FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fileSaver.SaveVoterList(this.GetSelectedPollingVenue().Persons, this.SelectedFolderPath(), this.ElectionName(), this.ElectionDate());
            }
        }

        private void FileOpenDialogImport(Object sender, EventArgs e)
        {
            form.OpenFileDialog.Filter = "Xml Files (*.xml)|*.xml";
            if (form.OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetPollingVenues(form.OpenFileDialog.FileName);
            }
        }

        private PollingVenue GetSelectedPollingVenue()
        {
            if (form.TableView.SelectedRows.Count > 0)
            {
                return pollingVenues[form.TableView.SelectedRows[0].Index];
            }else
            {
                return null;
            }
            
        }

        private void ErrorLoadFileDialog(Object sender, ValidationEventArgs e)
        {
            MessageBox.Show(e.Message, "XML Parsing Error", MessageBoxButtons.OK);
        }

        private string ElectionName()
        {
            return form.ElectionName.Text;
        }

        private string ElectionDate()
        {
            return form.ElectionDate.Text;
        }

        private string SelectedFolderPath()
        {
            return form.FolderBrowserDialog.SelectedPath;
        }

        private string SelectedFilePath()
        {
            return form.SaveFileDialog.FileName;
        }

        public void Run()
        {
           Application.Run(form);
        }
    }
        
}
