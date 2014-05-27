using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Reflection;
using Microsoft.Win32;
using Toopher;
using Shared.Settings;

namespace ToopherRdpConfigManager {
	class InstallUtil {

		// Initalized in the static constructor
		static readonly SecurityIdentifier ADMIN_GROUP;
		static readonly SecurityIdentifier USERS_GROUP;
		static readonly SecurityIdentifier SYSTEM_ACCT;
		static readonly SecurityIdentifier AUTHED_USERS;
		
		static InstallUtil()
        {
            // Init logging
            Toopher.Shared.Logging.LogManager.Init();
            
            // Intialize readonly variables

            ADMIN_GROUP = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            USERS_GROUP = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            SYSTEM_ACCT = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
            AUTHED_USERS = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
        }
		public static void DoInstall () {
			SetRegistryAcls ();
			RegisterAndEnableCredentialProvider ();
		}

		public static void DoUninstall () {
			// Uninstall CP
			UninstallCredentialProvider ();
		}

		private static void SetRegistryAcls () {
			string ToopherSubKey = Abstractions.Settings.DynamicSettings.ROOT_KEY;

			using(RegistryKey key = Registry.LocalMachine.CreateSubKey (ToopherSubKey)) {
				if(key != null) {
					
					RegistryAccessRule allowRead = new RegistryAccessRule (
						USERS_GROUP, RegistryRights.ReadKey,
						InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
						PropagationFlags.None, AccessControlType.Allow);
					RegistryAccessRule adminFull = new RegistryAccessRule (
						ADMIN_GROUP, RegistryRights.FullControl,
						InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
						PropagationFlags.None, AccessControlType.Allow);
					RegistryAccessRule systemFull = new RegistryAccessRule (
						SYSTEM_ACCT, RegistryRights.FullControl,
						InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
						PropagationFlags.None, AccessControlType.Allow);

					RegistrySecurity keySec = key.GetAccessControl ();

					// Remove inherited rules
					keySec.SetAccessRuleProtection (true, false);

					// Add full control for administrators and system.
					keySec.AddAccessRule (adminFull);
					keySec.AddAccessRule (systemFull);

					// Remove any read rules for users (if they exist)
					keySec.RemoveAccessRuleAll (allowRead);

					// Apply the rules..
					key.SetAccessControl (keySec);

				}
			}
		}

		private static void RegisterAndEnableCredentialProvider () {
			Toopher.CredentialProvider.Registration.CredProviderManager cpManager =
				Toopher.CredentialProvider.Registration.CredProviderManager.GetManager ();

			ToopherSettings tSettings = new ToopherSettings ();
			tSettings.ToopherAuthExePath = ConfigureSubOptions.DefaultAuthExePath();

			cpManager.CpInfo.OpMode = Toopher.CredentialProvider.Registration.OperationMode.INSTALL;
			cpManager.ExecuteDefaultAction ();
			
			cpManager.CpInfo.OpMode = Toopher.CredentialProvider.Registration.OperationMode.ENABLE;
			cpManager.ExecuteDefaultAction ();
		}

		private static void UninstallCredentialProvider () {
			Toopher.CredentialProvider.Registration.CredProviderManager cpManager =
				Toopher.CredentialProvider.Registration.CredProviderManager.GetManager ();

			cpManager.CpInfo.OpMode = Toopher.CredentialProvider.Registration.OperationMode.UNINSTALL;
			cpManager.ExecuteDefaultAction ();
		}

		public static bool CredentialProviderEnabled {
			get {
				return Toopher.CredentialProvider.Registration.CredProviderManager.GetManager ().Enabled ();
			}
		}

	}
}
