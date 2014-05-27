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
	public partial class LogUI : MetroForm {
		ILoggableJob job;
		public LogUI (string title) {
			InitializeComponent ();
			this.Text = title;
			this.job = null;
			this.Style = MetroFramework.MetroColorStyle.Black;
		}
		public LogUI (ILoggableJob job) {
			this.job = job;
			InitializeComponent ();
			this.Text = job.Name;
			job.Log += OnLog;
			job.Done += OnDone;
			btnClose.Text = "Cancel";
			this.Style = MetroFramework.MetroColorStyle.Black;
		}
		protected override void OnShown (EventArgs e) {
			base.OnShown (e);
			if((job != null) && (!job.Running)) {
				job.Run ();
			}
		}
		public void OnLog (String msg) {
			if(this.InvokeRequired) {
				this.Invoke ((Action)delegate { OnLog (msg); });
			} else {
				this.tbLog.AppendText (System.DateTime.Now.ToString().PadRight(40, ' ') +  msg + Environment.NewLine);
			}
		}

		public void OnDone () {
			if(this.InvokeRequired) {
				this.Invoke ((Action)delegate { OnDone (); });
			} else {
				btnClose.Text = "Close";
			}
		}

		private void btnClose_Click (object sender, EventArgs e) {
			if((job != null) && job.Running) {
				job.Abort ();
			}
			this.Close ();
		}
	}
}
