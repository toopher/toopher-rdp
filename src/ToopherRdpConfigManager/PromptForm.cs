using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Forms;

namespace ToopherRdpConfigManager {
	public partial class PromptForm : MetroForm {
		public PromptForm (String prompt) {
			InitializeComponent ();
			this.Text = prompt;
		}

		private void tbUserInput_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e) {
			if(e.KeyChar == '\r') {
				this.Close ();
			}
		}

		private void btnDone_Click (object sender, EventArgs e) {
			this.Close ();
		}
	}
}
