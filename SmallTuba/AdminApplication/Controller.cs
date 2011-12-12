namespace AdminApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml.Schema;

    using SmallTuba.Entities;
    using SmallTuba.IO;

    public class Controller
    {
        private List<PollingVenue> pollingVenues;
        private MainWindow form;
        private ExportWindow export;
        public event PollingVenuesChanged Changed;

        public delegate void PollingVenuesChanged();

        public Controller()
        {
            form = new MainWindow();
            export = new ExportWindow();
            this.InitializeEventSubscribers();
        }

        private void InitializeEventSubscribers()
        {
            form.ImportData.Click += this.FileOpenDialogImport;
            form.ExportData.Click += (o, e) => OpenExportWindow();
            export.ExportData.Click += ExportData;
            export.Cancel.Click += (o, e) => export.Close();
            Changed += this.UpdateTable;
        }

        private void SetPollingVenues(string path)
        {
            FileLoader fl = new FileLoader();
            pollingVenues = fl.GetPollingVenues(path, this.ErrorLoadFileDialog);
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
            }

            return null;

        }

        private bool PollingVenueSelected()
        {
            if (this.GetSelectedPollingVenue() == null)
            {
                return false;
            }
            return true;
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
            return export.FolderBrowserDialog.SelectedPath;
        }

        public void Run()
        {
           Application.Run(form);
        }

        private void OpenExportWindow()
        {
            if (this.PollingVenueSelected())
            {
                export.ShowDialog();
            }else
            {
                MessageBox.Show("No polling venue is selected", "Notification", MessageBoxButtons.OK);
            }
            
        }

        private void ExportData(Object sender, EventArgs e)
        {
            if(!this.ExportElementsSelected())
            {
                MessageBox.Show("No export elements are selected", "Notification", MessageBoxButtons.OK);
                return;
            }

            if (export.FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FileSaver fs = new FileSaver(this.SelectedFolderPath(), this.GetSelectedPollingVenue().PollingVenueAddress.Name);
                if (export.PollingCards.Checked)
                {
                    fs.SavePollingCards(this.GetSelectedPollingVenue(), this.ElectionName(), this.ElectionDate());
                }
                if (export.VoterLists.Checked)
                {
                    fs.SaveVoterList(this.GetSelectedPollingVenue().Persons, this.ElectionName(), this.ElectionDate());
                }
                if (export.Voters.Checked)
                {
                    fs.SaveVoters(this.GetSelectedPollingVenue());
                }
            }

            export.Close();
        }

        private bool ExportElementsSelected()
        {
            return export.PollingCards.Checked || export.VoterLists.Checked || export.Voters.Checked;
        }
    }
        
}
