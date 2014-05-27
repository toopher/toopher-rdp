using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Toopher;
using System.Reflection;
using Shared.Settings;

namespace ToopherAuth
{
	static class Program
	{

		private static AuthenticationStatusUI statusForm;
		public static void UpdateStatus (String status) {
			if(statusForm.InvokeRequired) {
				statusForm.Invoke((MethodInvoker)(() => { UpdateStatus (status); }));
			} else {
				String msg = DateTime.Now.ToShortTimeString () + ": " + status;
				statusForm.debugTextBox.AppendText (msg + Environment.NewLine);
			}
		}

		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main (String[] args)
		{
			ToopherSettings settings = new ToopherSettings ();
			if(!settings.IsConfigured) {
				MessageBox.Show ("Toopher CredentialProvider has not been configured.  Toopher Authentication is not currently active.  Please contact your administrator.");
				return AuthenticationJob.SUCCESS;
			}

			string userName = string.Empty;
			if(args.Length > 0) {
				userName = args[0];
			} else {
				userName = WinApiMethods.getTerminalInfoString (WinApiMethods.WTS_INFO_CLASS.WTSUserName);
			}

			if(string.IsNullOrEmpty (userName)) {
				return AuthenticationJob.SUCCESS;
			}

			AuthenticationJob job = new AuthenticationJob (userName, WinApiMethods.buildTerminalIdentifier(), start:false );
			
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			AuthenticationStatusUI form = new AuthenticationStatusUI (job);
			Application.Run (form);
			int result = job.GetAuthenticationResult();
			return result;
		}

	}
}
