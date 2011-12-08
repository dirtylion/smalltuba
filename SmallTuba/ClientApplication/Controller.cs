// -----------------------------------------------------------------------
// <copyright file="Controller.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ClientApplication
{
	using System;
	using System.Windows.Forms;
	using SmallTuba.Entities;
	using SmallTuba.Network.Voter;
	using SmallTuba.Utility;
 
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Controller
    {
        private WelcomeForm welcomeForm;
        private MainForm mainForm;
        private LogForm logForm;
        private VoterNetworkClient networkClient;
        private Person currentVoter;
        private Model model;

        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.welcomeForm = new WelcomeForm();
            this.mainForm = new MainForm();
            this.networkClient = new VoterNetworkClient();
            model = new Model();
            currentVoter = null;
        }

        public void Run()
        {
            SetListeners();
            SetDropDown();
            Application.Run(this.welcomeForm);
        }

        private void SetListeners()
        {
            this.welcomeForm.RefreshButton.Click += (object sender, EventArgs e) => SetDropDown();
            this.welcomeForm.OKButton.Click += (object sender, EventArgs e) => GoToMainForm();
            this.mainForm.IdSearchButton.Click += (object sender, EventArgs e) => SearchId();
            this.mainForm.CprSearchButton.Click += (object sender, EventArgs e) => SearchCpr();
            this.mainForm.LogButton.Click += (object sender, EventArgs e) => CreateLog();
            this.mainForm.RegisterButton.Click += (object sender, EventArgs e) => Register();
            this.mainForm.UnregisterButton.Click += (object sender, EventArgs e) => Unregister();
            this.mainForm.ClearButton.Click += (object sender, EventArgs e) => ClearVoter();
            this.mainForm.FormClosed += (object sender, FormClosedEventArgs e) => Application.Exit();
        }

        private void SetDropDown()
        {
            welcomeForm.dropdown.Items.Clear();
            string[] arr = networkClient.ValidTables();
            if(arr != null)
            {
                welcomeForm.dropdown.Items.AddRange(arr);
                welcomeForm.dropdown.SelectedIndex = 0;
                welcomeForm.OKButton.Enabled = true;
            }
            else
            {
                welcomeForm.dropdown.Items.Add("No connection");
                welcomeForm.OKButton.Enabled = false;
            }
        }

        private void GoToMainForm()
        {
            model.Name = this.welcomeForm.dropdown.SelectedItem.ToString();
            this.welcomeForm.Hide();
            this.networkClient.Name = model.Name;
            this.mainForm.ThisTable.Text = model.Name;
            ClearVoter();
            this.mainForm.Show();
        }

        private void SearchId()
        {
            try
            {
                int id = int.Parse(this.mainForm.IdTextBox.Text);
                Person person = networkClient.GetPersonFromId(id);
                SetVoter(person);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void SearchCpr()
        {
            try{
                int cpr = int.Parse(this.mainForm.CprTextBox.Text);
                Person person = networkClient.GetPersonFromCpr(cpr);
                SetVoter(person);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Invalid input");
            }
       }

        private void CreateLog()
        {
            this.logForm = new LogForm();
            this.logForm.LogListBox.Items.AddRange(model.Log.ToArray());
            this.logForm.TableLable.Text = model.Name;
            this.logForm.ChooseButton.Click += (object sender, EventArgs e) => ChooseLine();
            this.logForm.CloseButton.Click += (object sender, EventArgs e) => CloseLog();
            this.logForm.Show();
        }

        private void Register()
        {
            if (networkClient.RegisterVoter(currentVoter))
            {
                model.Log.Add(new ClientLog(currentVoter, "registered"));
                ClearVoter();
                MessageBox.Show("Succes!!!");
            }
            else
            {
                MessageBox.Show("Fail!!!");
            }
        }

        private void Unregister()
        {
            if (networkClient.UnregisterVoter(currentVoter))
            {
                model.Log.Add(new ClientLog(currentVoter, "unregistered"));
                ClearVoter();
                MessageBox.Show("Succes!!!");
            }
            else
            {
                MessageBox.Show("Fail!!!");
            }
            
        }

        private void ChooseLine()
        {
            // TODO: GET NEW VOTER FROM SERVER
            if (this.logForm.LogListBox.SelectedItem != null)
            {
                ClientLog logState = (ClientLog) this.logForm.LogListBox.SelectedItem;
                SetVoter(logState.Voter);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
                CloseLog();
            }
        }

        private void CloseLog()
        {
            this.logForm.Hide();
            this.logForm.Dispose();
            Console.Out.WriteLine("Close Log");
        }

        private void SetVoter(Person voter)
        {
            if(voter == null)
            {
                MessageBox.Show("No network connection");
            }
            else if(!voter.Exists)
            {
                MessageBox.Show("No voter found matching this criteria");
            }
            else
            {
                currentVoter = voter;
                this.mainForm.RegisterButton.Enabled = true;
                this.mainForm.UnregisterButton.Enabled = true;
                this.mainForm.ClearButton.Enabled = true;
                this.mainForm.ID.Text = voter.VoterId.ToString();
                this.mainForm.FirstName.Text = voter.FirstName;
                this.mainForm.LastName.Text = voter.LastName;
                this.mainForm.Cpr.Text = voter.Cpr.ToString();
                this.mainForm.Voted.Text = voter.Voted.ToString();
                if(voter.Voted)
                {
                    this.mainForm.Table.Text = voter.VotedPollingTable;
                    DateTime time = TimeConverter.ConvertFromUnixTimestamp(voter.VotedTime);
                    this.mainForm.Time.Text = time.ToLocalTime().Hour.ToString() + ":" + time.ToLocalTime().Minute.ToString();
                }
                else
                {
                    this.mainForm.Table.Text = "";
                    this.mainForm.Time.Text = "";
                }
            }
        }

        private void ClearVoter()
        {
            currentVoter = null;
            this.mainForm.IdTextBox.Text = "";
            this.mainForm.CprTextBox.Text = "";
            this.mainForm.RegisterButton.Enabled = false;
            this.mainForm.UnregisterButton.Enabled = false;
            this.mainForm.ClearButton.Enabled = false;
            this.mainForm.ID.Text = "";
            this.mainForm.FirstName.Text = "";
            this.mainForm.LastName.Text = "";
            this.mainForm.Cpr.Text = "";
            this.mainForm.Voted.Text = "";
            this.mainForm.Table.Text = "";
            this.mainForm.Time.Text = "";
        }
    }
}