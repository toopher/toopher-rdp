using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
namespace ToopherRdpConfigManager {
	public class GuiSubOptions {
	}

	public class InstallSubOptions {
		
	}

	public class UninstallSubOptions {
	}

	public class GetConfigSubOptions
	{
	}
	public class ConfigureSubOptions {
		[HelpOption]
		public string GetUsage () {
			return "Usage: api <key> <secret> [options]" + Environment.NewLine + HelpText.AutoBuild (this);
		}

		/*
		[Option('n', "suppress-check", Required=false, DefaultValue=false, HelpText="Suppress check when configuring new API credentials")]
		public bool SuppressCheck { get; set; }
		 * */

		[Option ('k', "key", DefaultValue = null, HelpText = "Toopher API Consumer Key", Required = false)]
		public String Key { get; set; }

		[Option ('s', "secret", DefaultValue = null, HelpText = "Toopher API Consumer Secret", Required = false)]
		public String Secret { get; set; }

		[Option ('b', "base-url", DefaultValue = null, HelpText = "Toopher API Base URL", Required = false)]
		public String BaseUrl { get; set; }

		[Option (longName: "set-default", MutuallyExclusiveSet="IsDefaultTile", DefaultValue=false, Required=false, HelpText = "Set Toopher as default Credential Provider")]
		public bool SetDefault { get; set; }

		[Option (longName: "unset-default", MutuallyExclusiveSet = "IsDefaultTile", DefaultValue = false, Required = false, HelpText = "Unset Toopher as default Credential Provider")]
		public bool UnsetDefault { get; set; }

		[Option (longName: "allow-inline-pairing", MutuallyExclusiveSet = "AllowInlinePairing", DefaultValue = false, Required = false, HelpText = "Allow inline pairing")]
		public bool AllowInlinePairing { get; set; }

		[Option (longName: "no-inline-pairing", MutuallyExclusiveSet = "AllowInlinePairing", DefaultValue = false, Required = false, HelpText = "Do not allow inline pairing")]
		public bool NoInlinePairing { get; set; }

		[Option (longName: "enable-debug-mode", MutuallyExclusiveSet = "DebugMode", DefaultValue = false, Required = false, HelpText = "Enable DebugMode for ToopherAuth UI")]
		public bool EnableDebugMode { get; set; }

		[Option (longName: "disable-debug-mode", MutuallyExclusiveSet = "DebugMode", DefaultValue = false, Required = false, HelpText = "Turn Off DebugMode for ToopherAuth UI")]
		public bool DisableDebugMode { get; set; }


		const string DEFAULT_AUTH_EXE_NAME = "ToopherAuth.exe";
		public static string DefaultAuthExePath () {
			return Path.Combine (Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location), DEFAULT_AUTH_EXE_NAME);
		}

		private string toopherAuthExePath = null;
		[Option ('x', "toopher-auth-exe", Required = false, DefaultValue = null, HelpText = "Path to ToopherAuth executable")]
		public string ToopherAuthExePath { get; set; }

	}

	class Options {

		[VerbOption("gui", HelpText="Open a GUI configuration prompt")]
		public GuiSubOptions GuiVerb { get; set; }

		[VerbOption("install", HelpText="Install Toopher Credential Provider")]
		public InstallSubOptions InstallVerb { get; set; }

		[VerbOption("uninstall", HelpText="Uninstall Toopher Credential Provider")]
		public UninstallSubOptions UninstallVerb { get; set; }

		[VerbOption("set", HelpText="Configure Toopher Credential Provider options")]
		public ConfigureSubOptions ConfigureApiVerb { get; set; }

		[VerbOption ("get", HelpText = "Show Toopher Credential Provider Options")]
		public GetConfigSubOptions GetConfigApiVerb { get; set; }

		[HelpOption]
		public string GetUsage () {
			return HelpText.AutoBuild (this);
		}

		[HelpVerbOption]
		public string GetUsage (string verb) {
			return HelpText.AutoBuild (this, verb);
		}
	}
}
