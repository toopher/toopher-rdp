namespace ToopherRdpConfigManager {
	partial class PromptForm {
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
			this.tbUserInput = new MetroFramework.Controls.MetroTextBox();
			this.btnDone = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// tbUserInput
			// 
			this.tbUserInput.Lines = new string[0];
			this.tbUserInput.Location = new System.Drawing.Point(23, 63);
			this.tbUserInput.MaxLength = 32767;
			this.tbUserInput.Name = "tbUserInput";
			this.tbUserInput.PasswordChar = '\0';
			this.tbUserInput.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.tbUserInput.SelectedText = "";
			this.tbUserInput.Size = new System.Drawing.Size(327, 23);
			this.tbUserInput.TabIndex = 1;
			this.tbUserInput.UseSelectable = true;
			this.tbUserInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUserInput_KeyPress);
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(381, 63);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(45, 23);
			this.btnDone.TabIndex = 2;
			this.btnDone.Text = "OK";
			this.btnDone.UseSelectable = true;
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// PromptForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(452, 112);
			this.Controls.Add(this.btnDone);
			this.Controls.Add(this.tbUserInput);
			this.Name = "PromptForm";
			this.Resizable = false;
			this.Text = "PromptForm";
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Controls.MetroButton btnDone;
		public MetroFramework.Controls.MetroTextBox tbUserInput;
	}
}