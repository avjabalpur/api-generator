namespace CodeGenreater
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbAuthentication = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLoginName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.rdoSqlServer = new System.Windows.Forms.RadioButton();
            this.rdoWidows = new System.Windows.Forms.RadioButton();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtDataBaseName = new System.Windows.Forms.TextBox();
            this.gbProjectType = new System.Windows.Forms.GroupBox();
            this.cbDal = new System.Windows.Forms.CheckBox();
            this.cbWcf = new System.Windows.Forms.CheckBox();
            this.cbWebApi = new System.Windows.Forms.CheckBox();
            this.gbProjectDetails = new System.Windows.Forms.GroupBox();
            this.btnProjectLocation = new System.Windows.Forms.Button();
            this.txtNameSpaceName = new System.Windows.Forms.TextBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblNameSpace = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.fdProjectLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cbAttributRouting = new System.Windows.Forms.CheckBox();
            this.gbAuthentication.SuspendLayout();
            this.gbProjectType.SuspendLayout();
            this.gbProjectDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(96, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(96, 123);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "DataBase Name";
            // 
            // gbAuthentication
            // 
            this.gbAuthentication.Controls.Add(this.label8);
            this.gbAuthentication.Controls.Add(this.label7);
            this.gbAuthentication.Controls.Add(this.txtLoginName);
            this.gbAuthentication.Controls.Add(this.txtPassword);
            this.gbAuthentication.Controls.Add(this.rdoSqlServer);
            this.gbAuthentication.Controls.Add(this.rdoWidows);
            this.gbAuthentication.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAuthentication.Location = new System.Drawing.Point(101, 267);
            this.gbAuthentication.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbAuthentication.Name = "gbAuthentication";
            this.gbAuthentication.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbAuthentication.Size = new System.Drawing.Size(400, 192);
            this.gbAuthentication.TabIndex = 6;
            this.gbAuthentication.TabStop = false;
            this.gbAuthentication.Text = "Authentication";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 144);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 102);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Login Name";
            // 
            // txtLoginName
            // 
            this.txtLoginName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtLoginName.Location = new System.Drawing.Point(179, 102);
            this.txtLoginName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Size = new System.Drawing.Size(197, 26);
            this.txtLoginName.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(179, 144);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(197, 26);
            this.txtPassword.TabIndex = 2;
            // 
            // rdoSqlServer
            // 
            this.rdoSqlServer.AccessibleRole = System.Windows.Forms.AccessibleRole.RowHeader;
            this.rdoSqlServer.AutoSize = true;
            this.rdoSqlServer.Location = new System.Drawing.Point(28, 66);
            this.rdoSqlServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoSqlServer.Name = "rdoSqlServer";
            this.rdoSqlServer.Size = new System.Drawing.Size(214, 24);
            this.rdoSqlServer.TabIndex = 1;
            this.rdoSqlServer.TabStop = true;
            this.rdoSqlServer.Text = "SqlServer Authentication";
            this.rdoSqlServer.UseVisualStyleBackColor = true;
            this.rdoSqlServer.CheckedChanged += new System.EventHandler(this.rdoSqlServer_CheckedChanged);
            // 
            // rdoWidows
            // 
            this.rdoWidows.AutoSize = true;
            this.rdoWidows.Location = new System.Drawing.Point(28, 23);
            this.rdoWidows.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoWidows.Name = "rdoWidows";
            this.rdoWidows.Size = new System.Drawing.Size(209, 24);
            this.rdoWidows.TabIndex = 0;
            this.rdoWidows.TabStop = true;
            this.rdoWidows.Text = "Windows Authentication";
            this.rdoWidows.UseVisualStyleBackColor = true;
            this.rdoWidows.CheckedChanged += new System.EventHandler(this.rdoWidows_CheckedChanged);
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(335, 55);
            this.txtServerName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(269, 22);
            this.txtServerName.TabIndex = 7;
            // 
            // txtDataBaseName
            // 
            this.txtDataBaseName.Location = new System.Drawing.Point(335, 129);
            this.txtDataBaseName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDataBaseName.Name = "txtDataBaseName";
            this.txtDataBaseName.Size = new System.Drawing.Size(269, 22);
            this.txtDataBaseName.TabIndex = 8;
            // 
            // gbProjectType
            // 
            this.gbProjectType.Controls.Add(this.cbAttributRouting);
            this.gbProjectType.Controls.Add(this.cbDal);
            this.gbProjectType.Controls.Add(this.cbWcf);
            this.gbProjectType.Controls.Add(this.cbWebApi);
            this.gbProjectType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProjectType.Location = new System.Drawing.Point(696, 50);
            this.gbProjectType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProjectType.Name = "gbProjectType";
            this.gbProjectType.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProjectType.Size = new System.Drawing.Size(303, 166);
            this.gbProjectType.TabIndex = 9;
            this.gbProjectType.TabStop = false;
            this.gbProjectType.Text = "ProjectType";
            // 
            // cbDal
            // 
            this.cbDal.AutoSize = true;
            this.cbDal.Location = new System.Drawing.Point(23, 121);
            this.cbDal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDal.Name = "cbDal";
            this.cbDal.Size = new System.Drawing.Size(170, 24);
            this.cbDal.TabIndex = 2;
            this.cbDal.Text = "DataAccess Layer";
            this.cbDal.UseVisualStyleBackColor = true;
            // 
            // cbWcf
            // 
            this.cbWcf.AutoSize = true;
            this.cbWcf.Location = new System.Drawing.Point(23, 81);
            this.cbWcf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbWcf.Name = "cbWcf";
            this.cbWcf.Size = new System.Drawing.Size(130, 24);
            this.cbWcf.TabIndex = 1;
            this.cbWcf.Text = "WCF Service";
            this.cbWcf.UseVisualStyleBackColor = true;
            // 
            // cbWebApi
            // 
            this.cbWebApi.AutoSize = true;
            this.cbWebApi.Location = new System.Drawing.Point(23, 38);
            this.cbWebApi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbWebApi.Name = "cbWebApi";
            this.cbWebApi.Size = new System.Drawing.Size(89, 24);
            this.cbWebApi.TabIndex = 0;
            this.cbWebApi.Text = "WebApi";
            this.cbWebApi.UseVisualStyleBackColor = true;
            // 
            // gbProjectDetails
            // 
            this.gbProjectDetails.Controls.Add(this.btnProjectLocation);
            this.gbProjectDetails.Controls.Add(this.txtNameSpaceName);
            this.gbProjectDetails.Controls.Add(this.txtProjectName);
            this.gbProjectDetails.Controls.Add(this.lblNameSpace);
            this.gbProjectDetails.Controls.Add(this.lblProjectName);
            this.gbProjectDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProjectDetails.Location = new System.Drawing.Point(553, 273);
            this.gbProjectDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProjectDetails.Name = "gbProjectDetails";
            this.gbProjectDetails.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProjectDetails.Size = new System.Drawing.Size(463, 186);
            this.gbProjectDetails.TabIndex = 10;
            this.gbProjectDetails.TabStop = false;
            this.gbProjectDetails.Text = "ProjectDetails";
            // 
            // btnProjectLocation
            // 
            this.btnProjectLocation.Location = new System.Drawing.Point(123, 121);
            this.btnProjectLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnProjectLocation.Name = "btnProjectLocation";
            this.btnProjectLocation.Size = new System.Drawing.Size(227, 43);
            this.btnProjectLocation.TabIndex = 9;
            this.btnProjectLocation.Text = "Project Location";
            this.btnProjectLocation.UseVisualStyleBackColor = true;
            this.btnProjectLocation.Click += new System.EventHandler(this.btnProjectLocation_Click);
            // 
            // txtNameSpaceName
            // 
            this.txtNameSpaceName.Location = new System.Drawing.Point(227, 62);
            this.txtNameSpaceName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNameSpaceName.Name = "txtNameSpaceName";
            this.txtNameSpaceName.Size = new System.Drawing.Size(217, 26);
            this.txtNameSpaceName.TabIndex = 8;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(227, 26);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(217, 26);
            this.txtProjectName.TabIndex = 7;
            // 
            // lblNameSpace
            // 
            this.lblNameSpace.AutoSize = true;
            this.lblNameSpace.Location = new System.Drawing.Point(37, 69);
            this.lblNameSpace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameSpace.Name = "lblNameSpace";
            this.lblNameSpace.Size = new System.Drawing.Size(147, 20);
            this.lblNameSpace.TabIndex = 6;
            this.lblNameSpace.Text = "Namespace Name";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(37, 26);
            this.lblProjectName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(111, 20);
            this.lblProjectName.TabIndex = 5;
            this.lblProjectName.Text = "Project Name";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(101, 491);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(733, 43);
            this.progressBar.TabIndex = 11;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(865, 491);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(151, 43);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(76, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1031, 559);
            this.panel1.TabIndex = 13;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(96, 588);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(92, 25);
            this.lblMessage.TabIndex = 14;
            this.lblMessage.Text = "message";
            // 
            // cbAttributRouting
            // 
            this.cbAttributRouting.AutoSize = true;
            this.cbAttributRouting.Checked = true;
            this.cbAttributRouting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAttributRouting.Enabled = false;
            this.cbAttributRouting.Location = new System.Drawing.Point(138, 38);
            this.cbAttributRouting.Margin = new System.Windows.Forms.Padding(4);
            this.cbAttributRouting.Name = "cbAttributRouting";
            this.cbAttributRouting.Size = new System.Drawing.Size(147, 24);
            this.cbAttributRouting.TabIndex = 3;
            this.cbAttributRouting.Text = "Attribut Routing";
            this.cbAttributRouting.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 636);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.gbProjectDetails);
            this.Controls.Add(this.gbProjectType);
            this.Controls.Add(this.txtDataBaseName);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.gbAuthentication);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "AmitFormGenreater";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbAuthentication.ResumeLayout(false);
            this.gbAuthentication.PerformLayout();
            this.gbProjectType.ResumeLayout(false);
            this.gbProjectType.PerformLayout();
            this.gbProjectDetails.ResumeLayout(false);
            this.gbProjectDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbAuthentication;
        private System.Windows.Forms.RadioButton rdoSqlServer;
        private System.Windows.Forms.RadioButton rdoWidows;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLoginName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.TextBox txtDataBaseName;
        private System.Windows.Forms.GroupBox gbProjectType;
        private System.Windows.Forms.CheckBox cbDal;
        private System.Windows.Forms.CheckBox cbWcf;
        private System.Windows.Forms.CheckBox cbWebApi;
        private System.Windows.Forms.GroupBox gbProjectDetails;
        private System.Windows.Forms.TextBox txtNameSpaceName;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label lblNameSpace;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.FolderBrowserDialog fdProjectLocation;
        private System.Windows.Forms.Button btnProjectLocation;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox cbAttributRouting;
    }
}

