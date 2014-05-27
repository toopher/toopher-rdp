namespace ToopherRdpConfigManager {
	partial class LogUI {
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
			this.tbLog = new MetroFramework.Controls.MetroTextBox();
			this.btnClose = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// tbLog
			// 
			this.tbLog.Location = new System.Drawing.Point(23, 63);
			this.tbLog.Multiline = true;
			this.tbLog.Name = "tbLog";
			this.tbLog.Size = new System.Drawing.Size(757, 214);
			this.tbLog.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(705, 294);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "metroButton1";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// LogUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(803, 340);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.tbLog);
			this.Name = "LogUI";
			this.Resizable = false;
			this.Text = "LogUI";
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Controls.MetroTextBox tbLog;
		private MetroFramework.Controls.MetroButton btnClose;
	}
}