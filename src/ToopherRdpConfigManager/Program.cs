using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared.Settings;
using CommandLine;

namespace ToopherRdpConfigManager {
	class Program {
		[STAThread]
		static void Main (string[] args) {
			Options options = new Options ();
			String invokedVerb = "";
			Object invokedVerbInstance = null;

			if(!CommandLine.Parser.Default.ParseArguments (args, options,
				(verb, subOptions) => {
					invokedVerb = verb;
					invokedVerbInstance = subOptions;
				})) 
			{
				RunGui ();
			}

			if(invokedVerbInstance is GuiSubOptions) {
				RunGui ();
			} else if (invokedVerbInstance is GetConfigSubOptions) {
				ToopherSettings cLib = new ToopherSettings ();

				Console.WriteLine ("ToopherBaseUrl        = " + cLib.ToopherBaseUrl ?? "(not configured)");
				Console.WriteLine ("ToopherConsumerKey    = " + cLib.ToopherConsumerKey ?? "(not configured)");
				Console.WriteLine ("ToopherConsumerSecret = " + cLib.ToopherConsumerSecret ?? "(not configured)");
				Console.WriteLine ("ToopherAuthExe        = " + cLib.ToopherAuthExePath ?? "(not configured)");
				Console.WriteLine ("IsDefault             = " + (cLib.IsDefaultTile ? "True" : "False"));
				Console.WriteLine ("AllowInlinePairing    = " + (cLib.AllowInlinePairing ? "True" : "False"));
				Console.WriteLine ("DebugMode             = " + (cLib.DebugMode ? "True" : "False"));
			} else if (invokedVerbInstance is ConfigureSubOptions) {
				ConfigureSubOptions o = invokedVerbInstance as ConfigureSubOptions;
				ToopherSettings cLib = new ToopherSettings ();

				if(!string.IsNullOrEmpty (o.Key)) {
					cLib.ToopherConsumerKey = o.Key;
				}
				if(!string.IsNullOrEmpty (o.Secret)) {
					cLib.ToopherConsumerSecret = o.Secret;
				}
				if(!string.IsNullOrEmpty (o.BaseUrl)) {
					cLib.ToopherBaseUrl = o.BaseUrl;
				}
				if(!string.IsNullOrEmpty (o.ToopherAuthExePath)) {
					cLib.ToopherAuthExePath = o.ToopherAuthExePath;
				} else {
					// make sure ToopherAuthExePath gets a default value if necessary
					if(string.IsNullOrEmpty (cLib.ToopherAuthExePath)) {
						cLib.ToopherAuthExePath = ConfigureSubOptions.DefaultAuthExePath ();
					}
				}
				if(o.SetDefault) {
					cLib.IsDefaultTile = true;
				}
				if(o.UnsetDefault) {
					cLib.IsDefaultTile = false;
				}
				if(o.AllowInlinePairing) {
					cLib.AllowInlinePairing = true;
				}
				if(o.NoInlinePairing) {
					cLib.AllowInlinePairing = false;
				}
				if(o.EnableDebugMode) {
					cLib.DebugMode = true;
				}
				if(o.DisableDebugMode) {
					cLib.DebugMode = false;
				}
			} else if(invokedVerbInstance is InstallSubOptions) {
				InstallUtil.DoInstall ();
			} else if(invokedVerbInstance is UninstallSubOptions) {
				InstallUtil.DoUninstall ();
			}
		}

		static private void RunGui () {
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			ConfigUI form = new ConfigUI ();
			Application.Run (form);
		}
	}
}
