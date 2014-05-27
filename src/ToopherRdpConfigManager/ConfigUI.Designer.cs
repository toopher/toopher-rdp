namespace ToopherRdpConfigManager {
	partial class ConfigUI {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
			this.tabConfigOptions = new MetroFramework.Controls.MetroTabPage();
			this.btnDiscard = new MetroFramework.Controls.MetroButton();
			this.btnTestApiCreds = new MetroFramework.Controls.MetroButton();
			this.btnSaveSettings = new MetroFramework.Controls.MetroButton();
			this.cbDebugMode = new MetroFramework.Controls.MetroCheckBox();
			this.cbSetDefaultCP = new MetroFramework.Controls.MetroCheckBox();
			this.cbAllowInlinePairing = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
			this.btnToggleCPInstall = new MetroFramework.Controls.MetroButton();
			this.lblCpInstalled = new MetroFramework.Controls.MetroLabel();
			this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
			this.tbApiUrl = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
			this.tbApiSecret = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
			this.tbApiKey = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.tabUserAdmin = new MetroFramework.Controls.MetroTabPage();
			this.pnlAdminUserStatus = new System.Windows.Forms.Panel();
			this.btnAdminUserToggleToopherEnabled = new MetroFramework.Controls.MetroButton();
			this.lblAdminUserToopherEnabled = new MetroFramework.Controls.MetroLabel();
			this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
			this.btnAdminUserPair = new MetroFramework.Controls.MetroButton();
			this.lblAdminUserPairingStatus = new MetroFramework.Controls.MetroLabel();
			this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
			this.btnAdminLoadUser = new MetroFramework.Controls.MetroButton();
			this.tbAdminUsername = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
			this.tabReadme = new MetroFramework.Controls.MetroTabPage();
			this.tbReadme = new MetroFramework.Controls.MetroTextBox();
			this.metroTabControl1.SuspendLayout();
			this.tabConfigOptions.SuspendLayout();
			this.tabUserAdmin.SuspendLayout();
			this.pnlAdminUserStatus.SuspendLayout();
			this.tabReadme.SuspendLayout();
			this.SuspendLayout();
			// 
			// metroTabControl1
			// 
			this.metroTabControl1.Controls.Add(this.tabConfigOptions);
			this.metroTabControl1.Controls.Add(this.tabUserAdmin);
			this.metroTabControl1.Controls.Add(this.tabReadme);
			this.metroTabControl1.Location = new System.Drawing.Point(23, 63);
			this.metroTabControl1.Name = "metroTabControl1";
			this.metroTabControl1.SelectedIndex = 2;
			this.metroTabControl1.Size = new System.Drawing.Size(812, 384);
			this.metroTabControl1.TabIndex = 0;
			this.metroTabControl1.UseSelectable = true;
			// 
			// tabConfigOptions
			// 
			this.tabConfigOptions.Controls.Add(this.btnDiscard);
			this.tabConfigOptions.Controls.Add(this.btnTestApiCreds);
			this.tabConfigOptions.Controls.Add(this.btnSaveSettings);
			this.tabConfigOptions.Controls.Add(this.cbDebugMode);
			this.tabConfigOptions.Controls.Add(this.cbSetDefaultCP);
			this.tabConfigOptions.Controls.Add(this.cbAllowInlinePairing);
			this.tabConfigOptions.Controls.Add(this.metroLabel6);
			this.tabConfigOptions.Controls.Add(this.btnToggleCPInstall);
			this.tabConfigOptions.Controls.Add(this.lblCpInstalled);
			this.tabConfigOptions.Controls.Add(this.metroLabel5);
			this.tabConfigOptions.Controls.Add(this.tbApiUrl);
			this.tabConfigOptions.Controls.Add(this.metroLabel4);
			this.tabConfigOptions.Controls.Add(this.tbApiSecret);
			this.tabConfigOptions.Controls.Add(this.metroLabel3);
			this.tabConfigOptions.Controls.Add(this.tbApiKey);
			this.tabConfigOptions.Controls.Add(this.metroLabel2);
			this.tabConfigOptions.Controls.Add(this.metroLabel1);
			this.tabConfigOptions.HorizontalScrollbarBarColor = true;
			this.tabConfigOptions.HorizontalScrollbarHighlightOnWheel = false;
			this.tabConfigOptions.HorizontalScrollbarSize = 10;
			this.tabConfigOptions.Location = new System.Drawing.Point(4, 38);
			this.tabConfigOptions.Name = "tabConfigOptions";
			this.tabConfigOptions.Size = new System.Drawing.Size(804, 342);
			this.tabConfigOptions.TabIndex = 0;
			this.tabConfigOptions.Text = "Configuration";
			this.tabConfigOptions.VerticalScrollbarBarColor = true;
			this.tabConfigOptions.VerticalScrollbarHighlightOnWheel = false;
			this.tabConfigOptions.VerticalScrollbarSize = 10;
			// 
			// btnDiscard
			// 
			this.btnDiscard.Location = new System.Drawing.Point(197, 319);
			this.btnDiscard.Name = "btnDiscard";
			this.btnDiscard.Size = new System.Drawing.Size(109, 23);
			this.btnDiscard.TabIndex = 18;
			this.btnDiscard.Text = "Discard Changes";
			this.btnDiscard.UseSelectable = true;
			this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
			// 
			// btnTestApiCreds
			// 
			this.btnTestApiCreds.Location = new System.Drawing.Point(113, 136);
			this.btnTestApiCreds.Name = "btnTestApiCreds";
			this.btnTestApiCreds.Size = new System.Drawing.Size(141, 23);
			this.btnTestApiCreds.TabIndex = 17;
			this.btnTestApiCreds.Text = "Test API Connectivity";
			this.btnTestApiCreds.UseSelectable = true;
			this.btnTestApiCreds.Click += new System.EventHandler(this.btnTestApiCreds_Click);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Location = new System.Drawing.Point(60, 319);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(112, 23);
			this.btnSaveSettings.TabIndex = 16;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.UseSelectable = true;
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// cbDebugMode
			// 
			this.cbDebugMode.AutoSize = true;
			this.cbDebugMode.Location = new System.Drawing.Point(60, 270);
			this.cbDebugMode.Name = "cbDebugMode";
			this.cbDebugMode.Size = new System.Drawing.Size(92, 15);
			this.cbDebugMode.TabIndex = 15;
			this.cbDebugMode.Text = "Debug Mode";
			this.cbDebugMode.UseSelectable = true;
			this.cbDebugMode.Click += new System.EventHandler(this.MarkDirty);
			// 
			// cbSetDefaultCP
			// 
			this.cbSetDefaultCP.AutoSize = true;
			this.cbSetDefaultCP.Location = new System.Drawing.Point(60, 233);
			this.cbSetDefaultCP.Name = "cbSetDefaultCP";
			this.cbSetDefaultCP.Size = new System.Drawing.Size(246, 15);
			this.cbSetDefaultCP.TabIndex = 14;
			this.cbSetDefaultCP.Text = "Set Toopher as Default Credential Provider";
			this.cbSetDefaultCP.UseSelectable = true;
			this.cbSetDefaultCP.Click += new System.EventHandler(this.MarkDirty);
			// 
			// cbAllowInlinePairing
			// 
			this.cbAllowInlinePairing.AutoSize = true;
			this.cbAllowInlinePairing.Location = new System.Drawing.Point(60, 211);
			this.cbAllowInlinePairing.Name = "cbAllowInlinePairing";
			this.cbAllowInlinePairing.Size = new System.Drawing.Size(125, 15);
			this.cbAllowInlinePairing.TabIndex = 13;
			this.cbAllowInlinePairing.Text = "Allow Inline Pairing";
			this.cbAllowInlinePairing.UseSelectable = true;
			this.cbAllowInlinePairing.Click += new System.EventHandler(this.MarkDirty);
			// 
			// metroLabel6
			// 
			this.metroLabel6.AutoSize = true;
			this.metroLabel6.Location = new System.Drawing.Point(60, 180);
			this.metroLabel6.Name = "metroLabel6";
			this.metroLabel6.Size = new System.Drawing.Size(55, 19);
			this.metroLabel6.TabIndex = 12;
			this.metroLabel6.Text = "Options";
			// 
			// btnToggleCPInstall
			// 
			this.btnToggleCPInstall.Location = new System.Drawing.Point(711, 74);
			this.btnToggleCPInstall.Name = "btnToggleCPInstall";
			this.btnToggleCPInstall.Size = new System.Drawing.Size(75, 23);
			this.btnToggleCPInstall.TabIndex = 11;
			this.btnToggleCPInstall.Text = "INSTALL";
			this.btnToggleCPInstall.UseSelectable = true;
			this.btnToggleCPInstall.Click += new System.EventHandler(this.btnToggleCPInstall_Click);
			// 
			// lblCpInstalled
			// 
			this.lblCpInstalled.AutoSize = true;
			this.lblCpInstalled.Location = new System.Drawing.Point(600, 76);
			this.lblCpInstalled.Name = "lblCpInstalled";
			this.lblCpInstalled.Size = new System.Drawing.Size(55, 19);
			this.lblCpInstalled.TabIndex = 10;
			this.lblCpInstalled.Text = "STATUS";
			// 
			// metroLabel5
			// 
			this.metroLabel5.AutoSize = true;
			this.metroLabel5.Location = new System.Drawing.Point(376, 76);
			this.metroLabel5.Name = "metroLabel5";
			this.metroLabel5.Size = new System.Drawing.Size(218, 19);
			this.metroLabel5.TabIndex = 9;
			this.metroLabel5.Text = "Toopher Credential Provider Status:";
			// 
			// tbApiUrl
			// 
			this.tbApiUrl.Lines = new string[] {
        "https://api.toopher.com/v1"};
			this.tbApiUrl.Location = new System.Drawing.Point(60, 107);
			this.tbApiUrl.MaxLength = 32767;
			this.tbApiUrl.Name = "tbApiUrl";
			this.tbApiUrl.PasswordChar = '\0';
			this.tbApiUrl.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbApiUrl.SelectedText = "";
			this.tbApiUrl.Size = new System.Drawing.Size(246, 23);
			this.tbApiUrl.TabIndex = 8;
			this.tbApiUrl.Text = "https://api.toopher.com/v1";
			this.tbApiUrl.UseSelectable = true;
			this.tbApiUrl.TextChanged += new System.EventHandler(this.MarkDirty);
			// 
			// metroLabel4
			// 
			this.metroLabel4.AutoSize = true;
			this.metroLabel4.Location = new System.Drawing.Point(3, 107);
			this.metroLabel4.Name = "metroLabel4";
			this.metroLabel4.Size = new System.Drawing.Size(32, 19);
			this.metroLabel4.TabIndex = 7;
			this.metroLabel4.Text = "URL";
			// 
			// tbApiSecret
			// 
			this.tbApiSecret.Lines = new string[] {
        "TOOPHER_CONSUMER_SECRET"};
			this.tbApiSecret.Location = new System.Drawing.Point(60, 76);
			this.tbApiSecret.MaxLength = 32767;
			this.tbApiSecret.Name = "tbApiSecret";
			this.tbApiSecret.PasswordChar = '\0';
			this.tbApiSecret.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbApiSecret.SelectedText = "";
			this.tbApiSecret.Size = new System.Drawing.Size(246, 23);
			this.tbApiSecret.TabIndex = 6;
			this.tbApiSecret.Text = "TOOPHER_CONSUMER_SECRET";
			this.tbApiSecret.UseSelectable = true;
			this.tbApiSecret.TextChanged += new System.EventHandler(this.MarkDirty);
			// 
			// metroLabel3
			// 
			this.metroLabel3.AutoSize = true;
			this.metroLabel3.Location = new System.Drawing.Point(3, 76);
			this.metroLabel3.Name = "metroLabel3";
			this.metroLabel3.Size = new System.Drawing.Size(45, 19);
			this.metroLabel3.TabIndex = 5;
			this.metroLabel3.Text = "Secret";
			// 
			// tbApiKey
			// 
			this.tbApiKey.Lines = new string[] {
        "TOOPHER_CONSUMER_KEY"};
			this.tbApiKey.Location = new System.Drawing.Point(60, 45);
			this.tbApiKey.MaxLength = 32767;
			this.tbApiKey.Name = "tbApiKey";
			this.tbApiKey.PasswordChar = '\0';
			this.tbApiKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbApiKey.SelectedText = "";
			this.tbApiKey.Size = new System.Drawing.Size(246, 23);
			this.tbApiKey.TabIndex = 4;
			this.tbApiKey.Text = "TOOPHER_CONSUMER_KEY";
			this.tbApiKey.UseSelectable = true;
			this.tbApiKey.TextChanged += new System.EventHandler(this.MarkDirty);
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.Location = new System.Drawing.Point(3, 45);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(29, 19);
			this.metroLabel2.TabIndex = 3;
			this.metroLabel2.Text = "Key";
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.Location = new System.Drawing.Point(60, 23);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(152, 19);
			this.metroLabel1.TabIndex = 2;
			this.metroLabel1.Text = "Toopher API Credentials";
			// 
			// tabUserAdmin
			// 
			this.tabUserAdmin.Controls.Add(this.pnlAdminUserStatus);
			this.tabUserAdmin.Controls.Add(this.btnAdminLoadUser);
			this.tabUserAdmin.Controls.Add(this.tbAdminUsername);
			this.tabUserAdmin.Controls.Add(this.metroLabel7);
			this.tabUserAdmin.HorizontalScrollbarBarColor = true;
			this.tabUserAdmin.HorizontalScrollbarHighlightOnWheel = false;
			this.tabUserAdmin.HorizontalScrollbarSize = 10;
			this.tabUserAdmin.Location = new System.Drawing.Point(4, 38);
			this.tabUserAdmin.Name = "tabUserAdmin";
			this.tabUserAdmin.Size = new System.Drawing.Size(804, 342);
			this.tabUserAdmin.TabIndex = 1;
			this.tabUserAdmin.Text = "User Administration";
			this.tabUserAdmin.VerticalScrollbarBarColor = true;
			this.tabUserAdmin.VerticalScrollbarHighlightOnWheel = false;
			this.tabUserAdmin.VerticalScrollbarSize = 10;
			// 
			// pnlAdminUserStatus
			// 
			this.pnlAdminUserStatus.Controls.Add(this.btnAdminUserToggleToopherEnabled);
			this.pnlAdminUserStatus.Controls.Add(this.lblAdminUserToopherEnabled);
			this.pnlAdminUserStatus.Controls.Add(this.metroLabel9);
			this.pnlAdminUserStatus.Controls.Add(this.btnAdminUserPair);
			this.pnlAdminUserStatus.Controls.Add(this.lblAdminUserPairingStatus);
			this.pnlAdminUserStatus.Controls.Add(this.metroLabel8);
			this.pnlAdminUserStatus.Location = new System.Drawing.Point(47, 64);
			this.pnlAdminUserStatus.Name = "pnlAdminUserStatus";
			this.pnlAdminUserStatus.Size = new System.Drawing.Size(467, 207);
			this.pnlAdminUserStatus.TabIndex = 6;
			// 
			// btnAdminUserToggleToopherEnabled
			// 
			this.btnAdminUserToggleToopherEnabled.Location = new System.Drawing.Point(324, 80);
			this.btnAdminUserToggleToopherEnabled.Name = "btnAdminUserToggleToopherEnabled";
			this.btnAdminUserToggleToopherEnabled.Size = new System.Drawing.Size(75, 23);
			this.btnAdminUserToggleToopherEnabled.TabIndex = 10;
			this.btnAdminUserToggleToopherEnabled.Text = "Disable Toopher";
			this.btnAdminUserToggleToopherEnabled.UseSelectable = true;
			this.btnAdminUserToggleToopherEnabled.Click += new System.EventHandler(this.btnAdminUserToggleToopherEnabled_Click);
			// 
			// lblAdminUserToopherEnabled
			// 
			this.lblAdminUserToopherEnabled.CausesValidation = false;
			this.lblAdminUserToopherEnabled.Location = new System.Drawing.Point(133, 82);
			this.lblAdminUserToopherEnabled.Name = "lblAdminUserToopherEnabled";
			this.lblAdminUserToopherEnabled.Size = new System.Drawing.Size(142, 19);
			this.lblAdminUserToopherEnabled.TabIndex = 9;
			// 
			// metroLabel9
			// 
			this.metroLabel9.AutoSize = true;
			this.metroLabel9.Location = new System.Drawing.Point(17, 82);
			this.metroLabel9.Name = "metroLabel9";
			this.metroLabel9.Size = new System.Drawing.Size(113, 19);
			this.metroLabel9.TabIndex = 8;
			this.metroLabel9.Text = "Toopher Enabled:";
			// 
			// btnAdminUserPair
			// 
			this.btnAdminUserPair.Location = new System.Drawing.Point(324, 52);
			this.btnAdminUserPair.Name = "btnAdminUserPair";
			this.btnAdminUserPair.Size = new System.Drawing.Size(75, 23);
			this.btnAdminUserPair.TabIndex = 7;
			this.btnAdminUserPair.Text = "Reset Pairing";
			this.btnAdminUserPair.UseSelectable = true;
			this.btnAdminUserPair.Click += new System.EventHandler(this.btnAdminPairUnpairUser_Click);
			// 
			// lblAdminUserPairingStatus
			// 
			this.lblAdminUserPairingStatus.Location = new System.Drawing.Point(133, 54);
			this.lblAdminUserPairingStatus.Name = "lblAdminUserPairingStatus";
			this.lblAdminUserPairingStatus.Size = new System.Drawing.Size(142, 19);
			this.lblAdminUserPairingStatus.TabIndex = 6;
			// 
			// metroLabel8
			// 
			this.metroLabel8.AutoSize = true;
			this.metroLabel8.Location = new System.Drawing.Point(17, 54);
			this.metroLabel8.Name = "metroLabel8";
			this.metroLabel8.Size = new System.Drawing.Size(91, 19);
			this.metroLabel8.TabIndex = 5;
			this.metroLabel8.Text = "Pairing Status:";
			// 
			// btnAdminLoadUser
			// 
			this.btnAdminLoadUser.Location = new System.Drawing.Point(435, 22);
			this.btnAdminLoadUser.Name = "btnAdminLoadUser";
			this.btnAdminLoadUser.Size = new System.Drawing.Size(126, 23);
			this.btnAdminLoadUser.TabIndex = 4;
			this.btnAdminLoadUser.Text = "Query Toopher API";
			this.btnAdminLoadUser.UseSelectable = true;
			this.btnAdminLoadUser.Click += new System.EventHandler(this.btnAdminLoadUser_Click);
			// 
			// tbAdminUsername
			// 
			this.tbAdminUsername.Lines = new string[0];
			this.tbAdminUsername.Location = new System.Drawing.Point(109, 22);
			this.tbAdminUsername.MaxLength = 32767;
			this.tbAdminUsername.Name = "tbAdminUsername";
			this.tbAdminUsername.PasswordChar = '\0';
			this.tbAdminUsername.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbAdminUsername.SelectedText = "";
			this.tbAdminUsername.Size = new System.Drawing.Size(179, 23);
			this.tbAdminUsername.TabIndex = 3;
			this.tbAdminUsername.UseSelectable = true;
			this.tbAdminUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAdminUsername_KeyPress);
			// 
			// metroLabel7
			// 
			this.metroLabel7.AutoSize = true;
			this.metroLabel7.Location = new System.Drawing.Point(14, 24);
			this.metroLabel7.Name = "metroLabel7";
			this.metroLabel7.Size = new System.Drawing.Size(71, 19);
			this.metroLabel7.TabIndex = 2;
			this.metroLabel7.Text = "Username:";
			// 
			// tabReadme
			// 
			this.tabReadme.Controls.Add(this.tbReadme);
			this.tabReadme.HorizontalScrollbarBarColor = true;
			this.tabReadme.HorizontalScrollbarHighlightOnWheel = false;
			this.tabReadme.HorizontalScrollbarSize = 10;
			this.tabReadme.Location = new System.Drawing.Point(4, 38);
			this.tabReadme.Name = "tabReadme";
			this.tabReadme.Size = new System.Drawing.Size(804, 342);
			this.tabReadme.TabIndex = 2;
			this.tabReadme.Text = "Readme";
			this.tabReadme.VerticalScrollbarBarColor = true;
			this.tabReadme.VerticalScrollbarHighlightOnWheel = false;
			this.tabReadme.VerticalScrollbarSize = 10;
			// 
			// tbReadme
			// 
			this.tbReadme.Lines = new string[] {
        "tbReadme"};
			this.tbReadme.Location = new System.Drawing.Point(4, 4);
			this.tbReadme.MaxLength = 32767;
			this.tbReadme.Multiline = true;
			this.tbReadme.Name = "tbReadme";
			this.tbReadme.PasswordChar = '\0';
			this.tbReadme.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbReadme.SelectedText = "";
			this.tbReadme.Size = new System.Drawing.Size(797, 325);
			this.tbReadme.TabIndex = 2;
			this.tbReadme.Text = "tbReadme";
			this.tbReadme.UseSelectable = true;
			// 
			// ConfigUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(858, 470);
			this.Controls.Add(this.metroTabControl1);
			this.Name = "ConfigUI";
			this.Resizable = false;
			this.Text = "Toopher RDP Configuration";
			this.metroTabControl1.ResumeLayout(false);
			this.tabConfigOptions.ResumeLayout(false);
			this.tabConfigOptions.PerformLayout();
			this.tabUserAdmin.ResumeLayout(false);
			this.tabUserAdmin.PerformLayout();
			this.pnlAdminUserStatus.ResumeLayout(false);
			this.pnlAdminUserStatus.PerformLayout();
			this.tabReadme.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Controls.MetroTabControl metroTabControl1;
		private MetroFramework.Controls.MetroTabPage tabConfigOptions;
		private MetroFramework.Controls.MetroCheckBox cbDebugMode;
		private MetroFramework.Controls.MetroCheckBox cbSetDefaultCP;
		private MetroFramework.Controls.MetroCheckBox cbAllowInlinePairing;
		private MetroFramework.Controls.MetroLabel metroLabel6;
		private MetroFramework.Controls.MetroButton btnToggleCPInstall;
		private MetroFramework.Controls.MetroLabel lblCpInstalled;
		private MetroFramework.Controls.MetroLabel metroLabel5;
		private MetroFramework.Controls.MetroTextBox tbApiUrl;
		private MetroFramework.Controls.MetroLabel metroLabel4;
		private MetroFramework.Controls.MetroTextBox tbApiSecret;
		private MetroFramework.Controls.MetroLabel metroLabel3;
		private MetroFramework.Controls.MetroTextBox tbApiKey;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroTabPage tabUserAdmin;
		private MetroFramework.Controls.MetroTabPage tabReadme;
		private MetroFramework.Controls.MetroButton btnSaveSettings;
		private MetroFramework.Controls.MetroButton btnTestApiCreds;
		private MetroFramework.Controls.MetroButton btnDiscard;
		private MetroFramework.Controls.MetroTextBox tbAdminUsername;
		private MetroFramework.Controls.MetroLabel metroLabel7;
		private MetroFramework.Controls.MetroButton btnAdminLoadUser;
		private System.Windows.Forms.Panel pnlAdminUserStatus;
		private MetroFramework.Controls.MetroButton btnAdminUserPair;
		private MetroFramework.Controls.MetroLabel lblAdminUserPairingStatus;
		private MetroFramework.Controls.MetroLabel metroLabel8;
		private MetroFramework.Controls.MetroLabel metroLabel9;
		private MetroFramework.Controls.MetroButton btnAdminUserToggleToopherEnabled;
		private MetroFramework.Controls.MetroLabel lblAdminUserToopherEnabled;
		private MetroFramework.Controls.MetroTextBox tbReadme;
	}
}