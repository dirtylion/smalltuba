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
            form.SaveFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (form.SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileSaver.SavePollingCards(this.GetPollingVenueFromTable(), this.SelectedFilePath(), this.ElectionName(), this.ElectionDate());
            }  
        }

        private void FolderBrowserVoterLists(Object sender, EventArgs e)
        {
            if (form.FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fileSaver.SaveVoterList(this.GetPollingVenueFromTable().Persons, this.SelectedFolderPath(), this.ElectionName(), this.ElectionDate());
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

        private PollingVenue GetPollingVenueFromTable()
        {
            return pollingVenues[form.TableView.SelectedRows[0].Index];
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
