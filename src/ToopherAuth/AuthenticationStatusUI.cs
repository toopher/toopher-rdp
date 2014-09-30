using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toopher;
using Shared.Settings;
using System.Windows.Forms;
using MetroFramework.Forms;
namespace ToopherAuth {
	public partial class AuthenticationStatusUI : MetroForm {
		private AuthenticationJob job;
		private bool debugMode = false;
		private bool inputPromptDisplayed = false;
		private ToopherSettings settings;
		public AuthenticationStatusUI (AuthenticationJob theJob) {
			this.job = theJob;
			settings = new ToopherSettings ();

			debugMode = settings.DebugMode;

			InitializeComponent ();

			debugTextBox.Visible = debugMode;

			job.InfoStatus += updateTitle;
			job.DebugStatus += updateStatus;
			job.Done += jobDone;
			job.PromptUser += promptUser;
		
			inputPanel.Visible = false;
			if(!job.IsRunning) {
				job.Start ();
			}
			doLayout ();
		}

		//protected override CreateParams CreateParams {
		//	get {
		//		CreateParams cp = base.CreateParams;
		//		cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
		//		return cp;
		//	}
		//} 
		const int VERTICAL_SPACING = 20;
		private void doLayout () {
			Control bottomControl = null;
			if(debugMode) {
				if(inputPanel.Visible) {
					debugTextBox.Top = inputPanel.Bottom + VERTICAL_SPACING;
				} else {
					debugTextBox.Top = inputPanel.Top;
				}
				bottomControl = debugTextBox;
			} else {
				if(inputPanel.Visible) {
					bottomControl = inputPanel;
				}
			}
			if(bottomControl != null) {
				this.Height = bottomControl.Bottom + VERTICAL_SPACING;
			} else {
				this.Height = 80;
			}

		}


		private void btnCancel_Click (object sender, EventArgs e) {
			if(!job.IsDone) {
				job.Cancel ();
			}
			this.Close ();
		}

		public void updateTitle (String status) {
			if(this.InvokeRequired) {
				this.Invoke ((MethodInvoker)(() => { updateTitle (status); }));
			} else {
				this.Text = status;
				this.Invalidate ();
				Thread.Sleep (1);  // if we don't call thread.sleep to give up our timeslice,
									// the form doesn't actually redraw
			}
		}

		public void updateStatus (String status) {
			if(!debugMode) {
				return;
			}
			if(debugTextBox.InvokeRequired) {
				debugTextBox.Invoke ((MethodInvoker)(() => { updateStatus (status); }));
			} else {
				String msg = DateTime.Now.ToShortTimeString () + ": " + status;
				debugTextBox.AppendText (msg + Environment.NewLine);
			}
		}

		private void jobDone (int result) {
			if(this.InvokeRequired) {
				this.Invoke ((MethodInvoker)(() => { jobDone (result); }));
			} else {
				if (!debugMode) {
					this.Close ();
				} else {
					// in debug mode, we don't want to actually exit.  Instead, hold the output on the screen so the user can examine the log
					btnCancel.Text = "Close";
				}
			}
		}

		private bool inputDone;

		private void setInputPrompt (String prompt) {
			if(this.InvokeRequired) {
				this.Invoke ((Action)delegate { setInputPrompt (prompt); });
			} else {
				inputPrompt.Text = prompt;
				inputTextBox.Text = "";
			}
		}
		private string getInputText () {
			if(this.InvokeRequired) {
				return (string)this.Invoke ((Func<string>)delegate { return getInputText (); });
			} else {
				return inputTextBox.Text;
			}
		}
		private string promptUser (String prompt) {
			string result = String.Empty; ;
			setInputPrompt (prompt);
			displayPrompt ();
			inputDone = false;
			while(!inputDone) {
				Thread.Sleep (1);
			}
			result = getInputText ();
			new Task (hidePrompt).Start ();
			return result;
		}
		private void inputTextBox_KeyPress (object sender, KeyPressEventArgs e) {
			if(e.KeyChar == '\r') {
				inputDone = true;
				e.Handled = true;
			}
		}


		private bool animateStepToward (int targetHeight) {
			if(this.InvokeRequired) {
				return (bool) this.Invoke ((Func<bool>)delegate { return animateStepToward (targetHeight); });
			} else {
				bool result = false;
				if(this.Height != targetHeight) {
					int heightStep = (this.Height > targetHeight) ? -5 : 5;

					if(Math.Abs (heightStep) < Math.Abs (this.Height - targetHeight)) {
						this.Height += heightStep;
					} else {
						this.Height = targetHeight;
						result = true;
					}
					this.Height += heightStep;
					if(debugMode) {
						debugTextBox.Top = this.Height - 240;
					}
				} else if(this.Height < targetHeight) {
					this.Height = this.Height + 1;
				} else {
					result = true;
				}
				return result;
			}
		}

		private void animateTowards (int targetHeight) {
			while(!animateStepToward (targetHeight)) {
				Thread.Sleep (10);
			}
		}

		private void animateDisplayPrompt () {
			int targetHeight;
			if(debugMode) {
				targetHeight = 420;
			} else {
				targetHeight = 180;
			}
			animateTowards (targetHeight);
		}

		private void animateHidePrompt () {
			int targetHeight;
			if(debugMode) {
				targetHeight = 330;
			} else {
				targetHeight = 90;
			}
			animateTowards (targetHeight);
		}

		private void metroButton1_Click (object sender, EventArgs e) {
			Action a = () => {
				if(inputPromptDisplayed) {
					hidePrompt ();
				} else {
					displayPrompt ();
				}
			};
			new Task (a).Start ();
		}

		private void setInputPromptVisible (bool visible) {
			if(this.InvokeRequired) {
				this.Invoke ((Action)delegate { setInputPromptVisible (visible); });
			} else {
				inputPanel.Visible = visible;
			}
		}

		private void displayPrompt () {
			animateDisplayPrompt ();
			setInputPromptVisible (true);
			inputPromptDisplayed = true;
		}

		private void hidePrompt () {
			setInputPromptVisible (false);
			animateHidePrompt ();
			inputPromptDisplayed = false;
		}

		
	}
}
