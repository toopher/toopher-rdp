using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework.Forms;
using Shared.Settings;

namespace ToopherRdpConfigManager {
	public partial class ConfigUI : MetroForm {
		private bool _formDirty = true;
		private bool FormDirty {
			get {
				return _formDirty;
			}
			set {
				_formDirty = value;
				btnSaveSettings.Enabled = _formDirty;
				btnDiscard.Enabled = _formDirty;
			}
		}
		public ConfigUI () {
			InitializeComponent ();
			metroTabControl1.SelectedTab = tabConfigOptions;
			LoadConfig ();
			UpdateInstallUI ();

			pnlAdminUserStatus.BackColor = this.BackColor;
			pnlAdminUserStatus.Visible = false;

			tbReadme.Lines = DATA.README.Split ('\n');
		}

		protected override void OnClosing (CancelEventArgs e) {
			if(FormDirty) {
				MetroFramework.MetroMessageBox.Show (this, "Please save or discard configuration changes before exiting", "Error");
				e.Cancel = true;
			} else {
				base.OnClosing (e);
			}
		}

		#region ConfigTab
		private void LoadConfig () {
			ToopherSettings settings = new ToopherSettings ();
			tbApiKey.Text = settings.ToopherConsumerKey;
			tbApiSecret.Text = settings.ToopherConsumerSecret;
			string configuredUrl = settings.ToopherBaseUrl;
			if(String.IsNullOrEmpty (configuredUrl)) {
				configuredUrl = Toopher.ToopherAPI.DEFAULT_BASE_URL;
			}
			tbApiUrl.Text = configuredUrl;

			cbAllowInlinePairing.Checked = settings.AllowInlinePairing;
			cbSetDefaultCP.Checked = settings.IsDefaultTile;
			cbDebugMode.Checked = settings.DebugMode;

			FormDirty = false;
		}

		private void MarkDirty (object sender, EventArgs e) {
			FormDirty = true;
		}

		private void btnTestApiCreds_Click (object sender, EventArgs e) {
			Toopher.ToopherAPI api = new Toopher.ToopherAPI (tbApiKey.Text, tbApiSecret.Text, baseUrl: tbApiUrl.Text);
			AsyncLoggableJob job = new AsyncLoggableJob (new TestApiJob (api));

			LogUI form = new LogUI (job);
			try {
				this.Enabled = false;
				form.ShowDialog ();
			} finally {
				this.Enabled = true;
			}
		}

		private void btnSaveSettings_Click (object sender, EventArgs e) {
			ToopherSettings settings = new ToopherSettings ();
			settings.ToopherConsumerKey = tbApiKey.Text;
			settings.ToopherConsumerSecret = tbApiSecret.Text;
			settings.ToopherBaseUrl = tbApiUrl.Text;

			settings.AllowInlinePairing = cbAllowInlinePairing.Checked;
			settings.IsDefaultTile = cbSetDefaultCP.Checked;
			settings.DebugMode = cbDebugMode.Checked;
		
			FormDirty = false;
		}

		private void btnDiscard_Click (object sender, EventArgs e) {
			LoadConfig ();
		}

		private bool CpInstalled {
			get {
				return InstallUtil.CredentialProviderEnabled;
			}
		}

		private void UpdateInstallUI () {
			if(CpInstalled) {
				lblCpInstalled.ForeColor = Color.Green;
				lblCpInstalled.Text = "Installed";
				btnToggleCPInstall.Text = "Uninstall";
			} else {
				lblCpInstalled.ForeColor = Color.Green;
				lblCpInstalled.Text = "NOT Installed";
				btnToggleCPInstall.Text = "Install";
			}
		}

		private void btnToggleCPInstall_Click (object sender, EventArgs e) {
			try {
				if(CpInstalled) {
					InstallUtil.DoUninstall ();
				} else {
					InstallUtil.DoInstall ();
				}
			} finally {
				UpdateInstallUI ();
			}
		}

		#endregion

		List<Toopher.PairingStatus> adminUserPairings = null;
		string adminUserName = String.Empty;
		bool adminUserIsKnown = false;
		bool adminUserPaired = false;
		bool adminUserEnabled = false;

		private void updatePairedStatusGuiElements () {

			lblAdminUserPairingStatus.Text = adminUserPaired ? "Paired" : "Not Paired";
			btnAdminUserPair.Text = adminUserPaired ? "Reset Pairing" : "Pair User";

			btnAdminUserToggleToopherEnabled.Enabled = adminUserIsKnown;
			lblAdminUserToopherEnabled.Text = adminUserEnabled ? "True" : "False";
			btnAdminUserToggleToopherEnabled.Text = adminUserEnabled ? "Disable" : "Enable";
		
		}
		private void loadUserPairingStatus () {
			if(this.InvokeRequired) {
				this.Invoke ((Action)delegate { loadUserPairingStatus (); });
			} else {
				adminUserName = tbAdminUsername.Text.ToLower ();
				ToopherSettings settings = new ToopherSettings ();

				Toopher.ToopherAPI api = new Toopher.ToopherAPI (settings.ToopherConsumerKey, settings.ToopherConsumerSecret, baseUrl: settings.ToopherBaseUrl);
				adminUserIsKnown = false;
				adminUserPaired = false;
				adminUserEnabled = false;
				try {
					IDictionary<string, object> userData = api.SearchForUser (adminUserName);
					adminUserPairings = api.GetUserPairings (adminUserName);
					adminUserPaired = adminUserPairings.Any ((p) => p.enabled && !(p["deactivated"].ToString ().ToLower () == "true"));
					if(userData.ContainsKey ("disable_toopher_auth")) {
						adminUserEnabled = userData["disable_toopher_auth"].ToString ().ToLower () != "true";
					} else {
						adminUserEnabled = true;
					}
					adminUserIsKnown = true;
				} catch(Toopher.RequestError err) {
					adminUserIsKnown = false;
					if(err.Message == "No users with name = " + adminUserName) {
						// effectively the same as a UserUnknownError - ignore
					} else {
						pnlAdminUserStatus.Visible = false;
						MetroFramework.MetroMessageBox.Show (this, "Error retrieving user data: " + err.Message);
					}
				}
				pnlAdminUserStatus.Visible = true;
				pnlAdminUserStatus.Enabled = true;
				updatePairedStatusGuiElements ();
			}
		}

		private void btnAdminLoadUser_Click (object sender, EventArgs e) {
			try {
				tbAdminUsername.Enabled = false;
				btnAdminLoadUser.Enabled = false;
				loadUserPairingStatus ();
			} finally {
				tbAdminUsername.Enabled = true;
				btnAdminLoadUser.Enabled = true;
			}
		}

		private void btnAdminPairUnpairUser_Click (object sender, EventArgs e) {
			ToopherSettings settings = new ToopherSettings ();
			Toopher.ToopherAPI api = new Toopher.ToopherAPI (settings.ToopherConsumerKey, settings.ToopherConsumerSecret, baseUrl: settings.ToopherBaseUrl);
					
			if(adminUserPaired) {
				try {
					pnlAdminUserStatus.Enabled = false;
					foreach(var pairing in adminUserPairings) {
						api.DeactivatePairing (pairing.id);
					}
				} finally {
					loadUserPairingStatus ();
				}
			} else {
				PromptForm f = new PromptForm ("Enter a Toopher Pairing Phrase");
				f.ShowDialog ();
				string pairingPhrase = f.tbUserInput.Text.ToLower ();

				LogUI logUI = new LogUI ("Pairing with Toopher");
				logUI.Show ();
				Action pairingTask = () => {
					try {
						Toopher.PairingStatus pairing = api.Pair (pairingPhrase, adminUserName);
						while(pairing.pending && !pairing.enabled) {
							Thread.Sleep (200);
							logUI.OnLog ("checking pairing status...");
							pairing = api.GetPairingStatus (pairing.id);
						}
						logUI.OnLog ("Pairing Result: " + (pairing.enabled ? "Enabled" : "Denied"));
					} finally {
						logUI.OnDone ();
						loadUserPairingStatus ();
					}
				};
				new Task (pairingTask).Start ();
					
			}


		}

		private void btnAdminUserToggleToopherEnabled_Click (object sender, EventArgs e) {
			try {
				if(adminUserIsKnown) {
					ToopherSettings settings = new ToopherSettings ();
					Toopher.ToopherAPI api = new Toopher.ToopherAPI (settings.ToopherConsumerKey, settings.ToopherConsumerSecret, baseUrl: settings.ToopherBaseUrl);
					api.SetToopherEnabledForUser (adminUserName, !adminUserEnabled);
				}
			} finally {
				loadUserPairingStatus ();
			}
		}

		private void tbAdminUsername_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e) {
			if(e.KeyChar == '\r') {
				btnAdminLoadUser_Click (null, null);
			}
		}

	
	}
}
