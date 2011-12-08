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
    
 
 
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Controller
    {
        private WelcomeForm welcomeForm;
        private MainForm mainForm;
        private LogForm logForm;
        private VoterNetworkClient networkClient;
        private PersonState currentVoter;
        private Model model;

        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.welcomeForm = new WelcomeForm();
            this.mainForm = new MainForm();
            this.networkClient = new VoterNetworkClient("Client");
            this.logForm = new LogForm();
            model = new Model();
            currentVoter = null;
        }

        public void Run()
        {
            SetListeners();
            GetData();
            Application.Run(this.welcomeForm);
        }

        private void SetListeners()
        {
            this.welcomeForm.RefreshButton.Click += new EventHandler(this.Refresh);
            this.welcomeForm.OKButton.Click += new EventHandler(this.Ok);
            this.mainForm.IdSearchButton.Click += new EventHandler(this.SearchId);
            this.mainForm.CprSearchButton.Click += new EventHandler(this.SearchCpr);
            this.mainForm.LogButton.Click += new EventHandler(this.Log);
            this.mainForm.RegisterButton.Click += new EventHandler(this.Register);
            this.mainForm.UnregisterButton.Click += new EventHandler(this.Unregister);
            this.mainForm.ClearButton.Click += new EventHandler(this.Clear);
            this.mainForm.FormClosed += new FormClosedEventHandler((object sender, FormClosedEventArgs e) => Application.Exit());
            this.logForm.ChooseButton.Click += new EventHandler(ChooseLog);
            this.logForm.CloseButton.Click += new EventHandler(CloseLog);
        }

        private void GetData()
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
                welcomeForm.OKButton.Enabled = false;
            }
        }

        private void Refresh(object o, EventArgs e)
        {
            Console.Out.WriteLine("Refreshed pressed");
            GetData();
        }

        private void Ok(object o, EventArgs e)
        {
            Console.Out.WriteLine("Ok pressed");
            this.mainForm.ThisTable.Text = this.welcomeForm.dropdown.SelectedItem.ToString();
            this.logForm.TableLable.Text = this.welcomeForm.dropdown.SelectedItem.ToString();
            GoToMainForm();
        }

        private void GoToMainForm()
        {
            this.welcomeForm.Hide();
            ClearVoter();
            this.mainForm.Show();
        }

        private void SearchId(object o, EventArgs e)
        {
            // TODO: Validate user input!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            try
            {
                int id = int.Parse(this.mainForm.IdTextBox.Text);
                PersonState person = networkClient.GetPersonFromId(id);
                SetVoter(person);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unvalid input");
            }
        }

        private void SearchCpr(object o, EventArgs e)
        {
            try{
                int cpr = int.Parse(this.mainForm.CprTextBox.Text);
                PersonState person = networkClient.GetPersonFromCpr(cpr);
                SetVoter(person);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unvalid input");
            }
       }

        private void Log(object o, EventArgs e)
        {
            this.logForm.LogListBox.Items.AddRange(model.Log.ToArray());
            this.logForm.Show();
        }

        private void Register(object o, EventArgs e)
        {
            if (networkClient.RegisterVoter(currentVoter))
            {
                model.Log.Add(new LogState(currentVoter, "registered"));
                MessageBox.Show("Succes!!!");
            }
            else
            {
                MessageBox.Show("Fail!!!");
            }
        }

        private void Unregister(object o, EventArgs e)
        {
            if (networkClient.UnregisterVoter(currentVoter))
            {
                model.Log.Add(new LogState(currentVoter, "unregistered"));
                MessageBox.Show("Succes!!!");
            }
            else
            {
                MessageBox.Show("Fail!!!");
            }
            
        }

        private void ChooseLog(object o, EventArgs e)
        {
            LogState logState = (LogState) this.logForm.LogListBox.SelectedItem;
            SetVoter(logState.Voter);
            this.logForm.Hide();
        }

        private void CloseLog(object o, EventArgs e)
        {
            this.logForm.Hide();
        }

        private void Clear(object o, EventArgs e)
        {
            ClearVoter();
        }

        private void SetVoter(PersonState voter)
        {
            if(voter != null)
            {
                currentVoter = voter;
                this.mainForm.RegisterButton.Enabled = true;
                this.mainForm.UnregisterButton.Enabled = true;
                this.mainForm.ClearButton.Enabled = true;
                this.mainForm.ID.Text = voter.Id.ToString();
                this.mainForm.FirstName.Text = voter.Firstname;
                this.mainForm.LastName.Text = voter.Lastname;
                this.mainForm.Cpr.Text = voter.Cpr.ToString();
                this.mainForm.Voted.Text = voter.Voted.ToString();
                this.mainForm.Table.Text = voter.PollingTable;
                // out commented for server testing, do not have the timeconverter yet, therefore I am not fixing it - Henrik
				// this.mainForm.Time.Text = voter.VotedTime.ToLocalTime().Hour.ToString() + ":" + voter.VotedTime.ToLocalTime().Minute.ToString();
            }
            else
            {
                MessageBox.Show("No voter found matching this criteria");
            }
        }

        private void ClearVoter()
        {
            currentVoter = null;
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
