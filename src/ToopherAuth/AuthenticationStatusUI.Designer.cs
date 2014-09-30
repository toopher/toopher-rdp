namespace ToopherAuth {
	partial class AuthenticationStatusUI {
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
			this.btnCancel = new MetroFramework.Controls.MetroButton();
			this.metroButton1 = new MetroFramework.Controls.MetroButton();
			this.inputTextBox = new MetroFramework.Controls.MetroTextBox();
			this.inputPrompt = new MetroFramework.Controls.MetroLabel();
			this.debugTextBox = new MetroFramework.Controls.MetroTextBox();
			this.inputPanel = new System.Windows.Forms.Panel();
			this.inputPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(529, 20);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(109, 41);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel Logon";
			this.btnCancel.UseSelectable = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// metroButton1
			// 
			this.metroButton1.Location = new System.Drawing.Point(681, 29);
			this.metroButton1.Name = "metroButton1";
			this.metroButton1.Size = new System.Drawing.Size(75, 23);
			this.metroButton1.TabIndex = 1;
			this.metroButton1.Text = "metroButton1";
			this.metroButton1.UseSelectable = true;
			this.metroButton1.Visible = false;
			this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
			// 
			// inputTextBox
			// 
			this.inputTextBox.Lines = new string[] {
        "metroTextBox1"};
			this.inputTextBox.Location = new System.Drawing.Point(3, 34);
			this.inputTextBox.MaxLength = 32767;
			this.inputTextBox.Name = "inputTextBox";
			this.inputTextBox.PasswordChar = '\0';
			this.inputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.inputTextBox.SelectedText = "";
			this.inputTextBox.Size = new System.Drawing.Size(369, 23);
			this.inputTextBox.TabIndex = 6;
			this.inputTextBox.Text = "metroTextBox1";
			this.inputTextBox.UseSelectable = true;
			this.inputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputTextBox_KeyPress);
			// 
			// inputPrompt
			// 
			this.inputPrompt.AutoSize = true;
			this.inputPrompt.Location = new System.Drawing.Point(3, 12);
			this.inputPrompt.Name = "inputPrompt";
			this.inputPrompt.Size = new System.Drawing.Size(83, 19);
			this.inputPrompt.TabIndex = 5;
			this.inputPrompt.Text = "metroLabel1";
			// 
			// debugTextBox
			// 
			this.debugTextBox.Lines = new string[0];
			this.debugTextBox.Location = new System.Drawing.Point(23, 164);
			this.debugTextBox.MaxLength = 32767;
			this.debugTextBox.Multiline = true;
			this.debugTextBox.Name = "debugTextBox";
			this.debugTextBox.PasswordChar = '\0';
			this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.debugTextBox.SelectedText = "";
			this.debugTextBox.Size = new System.Drawing.Size(615, 217);
			this.debugTextBox.TabIndex = 7;
			this.debugTextBox.UseSelectable = true;
			this.debugTextBox.Visible = false;
			// 
			// inputPanel
			// 
			this.inputPanel.Controls.Add(this.inputPrompt);
			this.inputPanel.Controls.Add(this.inputTextBox);
			this.inputPanel.Location = new System.Drawing.Point(23, 79);
			this.inputPanel.Name = "inputPanel";
			this.inputPanel.Size = new System.Drawing.Size(498, 69);
			this.inputPanel.TabIndex = 8;
			this.inputPanel.Visible = false;
			// 
			// AuthenticationStatusUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 470);
			this.ControlBox = false;
			this.Controls.Add(this.inputPanel);
			this.Controls.Add(this.debugTextBox);
			this.Controls.Add(this.metroButton1);
			this.Controls.Add(this.btnCancel);
			this.Name = "AuthenticationStatusUI";
			this.Text = "Authenticating with Toopher...";
			this.inputPanel.ResumeLayout(false);
			this.inputPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Controls.MetroButton btnCancel;
		private MetroFramework.Controls.MetroButton metroButton1;
		private MetroFramework.Controls.MetroTextBox inputTextBox;
		private MetroFramework.Controls.MetroLabel inputPrompt;
		public MetroFramework.Controls.MetroTextBox debugTextBox;
		private System.Windows.Forms.Panel inputPanel;
	}
}