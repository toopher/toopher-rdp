using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abstractions.Settings;

namespace Shared.Settings {
	public class ToopherSettings {
		dynamic settings;
		dynamic encryptedSettings;

		public bool IsConfigured {
			get {
				return !(string.IsNullOrEmpty (ToopherConsumerKey) || string.IsNullOrEmpty (ToopherConsumerSecret));
			}
		}

		public ToopherSettings () {
			settings = new DynamicSettings ();
			encryptedSettings = new EncryptedDynamicSettings ();
		}

		public string ToopherConsumerKey {
			get {
				try {
					return settings.ToopherConsumerKey;
				} catch(Exception e) {
					return null;
				}
			}
			set {
				settings.ToopherConsumerKey = value;
			}
		}
		public string ToopherConsumerSecret {
			get {
				try {
					return encryptedSettings.ToopherConsumerSecret;
				} catch(Exception e) {
					return null;
				}
			}
			set {
				encryptedSettings.ToopherConsumerSecret = value;
			}
		}
		public string ToopherBaseUrl {
			get {
				try {
					return settings.ToopherBaseUrl;
				} catch(Exception e) {
					return null;
				}
			}
			set {
				settings.ToopherBaseUrl = value;
			}
		}
		public string ToopherAuthExePath {
			get {
				try {
					return settings.ToopherAuthExePath;
				} catch(Exception e) {
					return null;
				}
			}
			set {
				settings.ToopherAuthExePath = value;
			}
		}

		public bool IsDefaultTile {
			get {
				try {
					return settings.CredentialProviderDefaultTile;
				} catch(Exception e) {
					return false;
				}
			}
			set {
				settings.CredentialProviderDefaultTile = value;
			}
		}

		public bool AllowInlinePairing {
			get {
				try {
					return settings.AllowInlinePairing;
				} catch(Exception e) {
					return true;
				}
			}
			set {
				settings.AllowInlinePairing = value;
			}
		}

		public bool DebugMode {
			get {
				try {
					return settings.DebugMode;
				} catch(Exception e) {
					return false;
				}
			}
			set {
				settings.DebugMode = value;
			}
		}
	}
}
