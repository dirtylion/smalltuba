namespace AdminApplication
{
    using System.Windows.Forms;

    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public Button ImportData
        {
            get
            {
                return importData;
            }
        }

        public Button ExportData
        {
            get
            {
                return exportData;
            }
        }

        public Button GeneratePollingCards
        {
            get
            {
                return generatePollingCards;
            }
        }

        public Button GenerateVoterList
        {
            get
            {
                return generateVoterList;
            }
        }

        public SaveFileDialog SaveFileDialog
        {
            get
            {
                return saveFileDialog;
            }
        }

        public DataGridView TableView
        {
            get
            {
                return tableView;
            }
        }

        public DateTimePicker ElectionDate
        {
            get
            {
                return electionDate;
            }
        }

        public TextBox ElectionName
        {
            get
            {
                return electionName;
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.electionName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.electionDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.importData = new System.Windows.Forms.Button();
            this.exportData = new System.Windows.Forms.Button();
            this.generatePollingCards = new System.Windows.Forms.Button();
            this.generateVoterList = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tableView = new System.Windows.Forms.DataGridView();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.tableView)).BeginInit();
            this.SuspendLayout();
            // 
            // electionName
            // 
            this.electionName.Location = new System.Drawing.Point(105, 35);
            this.electionName.Name = "electionName";
            this.electionName.Size = new System.Drawing.Size(100, 20);
            this.electionName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Election Name";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // electionDate
            // 
            this.electionDate.Location = new System.Drawing.Point(105, 74);
            this.electionDate.Name = "electionDate";
            this.electionDate.Size = new System.Drawing.Size(200, 20);
            this.electionDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Election Date";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // importData
            // 
            this.importData.Location = new System.Drawing.Point(340, 111);
            this.importData.Name = "importData";
            this.importData.Size = new System.Drawing.Size(130, 23);
            this.importData.TabIndex = 6;
            this.importData.Text = "Import Data";
            this.importData.UseVisualStyleBackColor = true;
            this.importData.Click += new System.EventHandler(this.button1_Click);
            // 
            // exportData
            // 
            this.exportData.Location = new System.Drawing.Point(340, 152);
            this.exportData.Name = "exportData";
            this.exportData.Size = new System.Drawing.Size(130, 23);
            this.exportData.TabIndex = 7;
            this.exportData.Text = "Export Data";
            this.exportData.UseVisualStyleBackColor = true;
            // 
            // generatePollingCards
            // 
            this.generatePollingCards.Location = new System.Drawing.Point(340, 195);
            this.generatePollingCards.Name = "generatePollingCards";
            this.generatePollingCards.Size = new System.Drawing.Size(130, 23);
            this.generatePollingCards.TabIndex = 8;
            this.generatePollingCards.Text = "Generate Polling Cards";
            this.generatePollingCards.UseVisualStyleBackColor = true;
            this.generatePollingCards.Click += new System.EventHandler(this.button3_Click);
            // 
            // generateVoterList
            // 
            this.generateVoterList.Location = new System.Drawing.Point(340, 237);
            this.generateVoterList.Name = "generateVoterList";
            this.generateVoterList.Size = new System.Drawing.Size(130, 23);
            this.generateVoterList.TabIndex = 9;
            this.generateVoterList.Text = "Generate Voter List";
            this.generateVoterList.UseVisualStyleBackColor = true;
            // 
            // tableView
            // 
            this.tableView.AllowUserToAddRows = false;
            this.tableView.AllowUserToDeleteRows = false;
            this.tableView.AllowUserToResizeRows = false;
            this.tableView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Location,
            this.Address});
            this.tableView.Cursor = System.Windows.Forms.Cursors.Default;
            this.tableView.Location = new System.Drawing.Point(33, 152);
            this.tableView.MultiSelect = false;
            this.tableView.Name = "tableView";
            this.tableView.ReadOnly = true;
            this.tableView.RowHeadersVisible = false;
            this.tableView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.tableView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView.Size = new System.Drawing.Size(240, 150);
            this.tableView.TabIndex = 10;
            this.tableView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            this.Location.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Address
            // 
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 319);
            this.Controls.Add(this.tableView);
            this.Controls.Add(this.generateVoterList);
            this.Controls.Add(this.generatePollingCards);
            this.Controls.Add(this.exportData);
            this.Controls.Add(this.importData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.electionDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.electionName);
            this.Name = "Form1";
            this.Text = "Name";
            ((System.ComponentModel.ISupportInitialize)(this.tableView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox electionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker electionDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button importData;
        private System.Windows.Forms.Button exportData;
        private System.Windows.Forms.Button generatePollingCards;
        private System.Windows.Forms.Button generateVoterList;
        private SaveFileDialog saveFileDialog;
        private DataGridView tableView;
        private DataGridViewTextBoxColumn Location;
        private DataGridViewTextBoxColumn Address;
    }
}

