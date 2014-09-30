using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toopher;
using Shared.Settings;

namespace ToopherAuth {
	public class AuthenticationJob {

		public Action<String> InfoStatus { get; set; }
		public Action<string> DebugStatus { get; set; }
		public Action<int> Done { get; set; }
		public Func<string, string> PromptUser { get; set; }
		public bool IsRunning { get; private set; }
		public bool IsDone { get; private set; }
		public bool IsCancelled { get; private set; }
		
		public static int SUCCESS = 0;
		public static int FAILURE = 1;

		enum STATE {
			AUTHENTICATE,
			POLL_FOR_AUTHENTICATION,
			ENTER_OTP,
			EVALUATE_AUTHENTICATION_STATUS,
			PAIR,
			POLL_FOR_PAIRING,
			USER_DISABLED,
			NAME_TERMINAL,
			RESET_PAIRING,
		}

		private const string DEFAULT_STATE_TITLE = "Authenticating with Toopher...";
		private static Dictionary<STATE, string> stateTitles = new Dictionary<STATE, string> () {
			{STATE.ENTER_OTP, "Authenticate with OTP"},
			{STATE.PAIR, "Pairing with Toopher"},
			{STATE.POLL_FOR_PAIRING, "Authorize pairing on mobile device"},
		};

		private ToopherAPI api;
		private int toopherAuthResult {get; set;}
		private string userName { get; set; }
		private string terminalIdentifier;
		private ToopherSettings toopherSettings;
		
		public AuthenticationJob (string userName, string terminalIdentifier, bool start = false) {
			this.userName = userName.ToLower();
			this.terminalIdentifier = terminalIdentifier;
			this.IsRunning = false;
			this.IsDone = false;
			this.IsCancelled = false;
			toopherSettings = new ToopherSettings ();
			String baseUrl = null;
			if(!string.IsNullOrEmpty(toopherSettings.ToopherBaseUrl)) {
				baseUrl = toopherSettings.ToopherBaseUrl;
			}
			this.api = new ToopherAPI (toopherSettings.ToopherConsumerKey, toopherSettings.ToopherConsumerSecret, baseUrl: baseUrl);
			if(start) {
				Start ();
			}
		}
		
		public int GetAuthenticationResult () {
			if(IsCancelled) {
				return FAILURE;
			}
			while(!IsDone) {
				Thread.Sleep (1);
			}
			return toopherAuthResult;
		}
		public void Cancel () {
			this.IsCancelled = true;
		}

		object runningLock = new object ();
		public void Start () {
			lock(runningLock) {
				if(!this.IsRunning) {
					this.IsRunning = true;
					new Task (RunToopherStateMachine).Start ();
				}
			}
		}

		private void OnInfoUpdate (string msg) {
			if(this.InfoStatus != null) {
				this.InfoStatus (msg);
			}
		}

		private void OnDebugStatus (string msg) {
			if(this.DebugStatus != null) {
				this.DebugStatus (msg);
			}
		}
		private void OnDone (int result) {
			this.IsDone = true;
			if(this.Done != null) {
				this.Done (result);
			}
		}
		private string OnPromptUser (string prompt) {
			if(this.PromptUser != null) {
				return this.PromptUser (prompt);
			} else {
				throw new ApplicationException ("Unable to prompt user: no handler!");
			}
		}

		private void RunToopherStateMachine () {
			STATE state = STATE.AUTHENTICATE;
			AuthenticationStatus authStatus = null;
			PairingStatus pairingStatus = null;

			OnInfoUpdate (DEFAULT_STATE_TITLE);
			OnDebugStatus ("Authenticating with Toopher");
			
			bool done = false;
			STATE lastState = state;
			while(!(done || IsCancelled)) {
				OnDebugStatus ("State: " + Enum.GetName (typeof (STATE), state));
				if(lastState != state) {
					if(stateTitles.ContainsKey (state)) {
						OnInfoUpdate (stateTitles[state]);
					} else {
						OnInfoUpdate (DEFAULT_STATE_TITLE);
					}
					lastState = state;
				}
				switch(state) {
					case STATE.AUTHENTICATE: {
							try {
								OnDebugStatus (String.Format ("Authenticating username = {0}, termId = {1}", userName, terminalIdentifier));
								authStatus = api.AuthenticateByUserName (userName, terminalIdentifier);
								OnDebugStatus (String.Format ("  Received Authentication ID = {0}", authStatus.id));
								state = STATE.EVALUATE_AUTHENTICATION_STATUS;
							} catch(UserDisabledError e) {
								OnDebugStatus ("  UserDisabledError");
								state = STATE.USER_DISABLED;
							} catch(UserUnknownError e) {
								OnDebugStatus (" UserUnknownError");
								state = STATE.PAIR;
							} catch(TerminalUnknownError e) {
								OnDebugStatus ("  TerminalUnknownError");
								state = STATE.NAME_TERMINAL;
							} catch(PairingDeactivatedError e) {
								OnDebugStatus ("  PairingDeactivatedError");
								state = STATE.PAIR;
							} catch(RequestError e) {
								OnDebugStatus ("Error communicating with Toopher API: " + e.Message);
							}
							break;
						};
					case STATE.POLL_FOR_AUTHENTICATION: {
							try {
								OnDebugStatus ("Checking Authentication Status...");
								authStatus = api.GetAuthenticationStatus (authStatus.id);
								state = STATE.EVALUATE_AUTHENTICATION_STATUS;
							} catch(RequestError err) {
								OnDebugStatus (String.Format ("Could not check authentication status (reason:{0})", err.Message));
							}

							break;
						};
					case STATE.EVALUATE_AUTHENTICATION_STATUS: {
							if(authStatus.pending) {
								state = STATE.POLL_FOR_AUTHENTICATION;
							} else {
								string automation = authStatus.automated ? "automatically " : "";
								toopherAuthResult = authStatus.granted ? SUCCESS : FAILURE;
								done = true;
								OnDebugStatus ("The request was " + automation + (toopherAuthResult == SUCCESS ? "GRANTED" : "DENIED") + "!");
								OnDebugStatus ("This request " + ((bool)authStatus["totp_valid"] ? "had" : "DID NOT HAVE") + " a valid authenticator OTP.");

							}
							break;
						};
					case STATE.ENTER_OTP: {
							string otp = OnPromptUser("Please enter the Pairing OTP value generated in the Toopher Mobile App:");
							authStatus = api.GetAuthenticationStatus (authStatus.id, otp: otp);
							state = STATE.EVALUATE_AUTHENTICATION_STATUS;
							break;
						};
					case STATE.USER_DISABLED: {
							OnDebugStatus ("User marked as Disabled");
							toopherAuthResult = SUCCESS;
							done = true;
							break;
						};

					case STATE.PAIR: {
							if(!toopherSettings.AllowInlinePairing) {
								OnDebugStatus ("No existing pairing for user, and inline pairing is not allowed");
								toopherAuthResult = FAILURE;
								done = true;
							} else {
								string pairingPhrase = String.Empty;
								while(pairingPhrase.Length == 0) {
									pairingPhrase = OnPromptUser ("Enter Pairing Phrase");
								}

								try {
									pairingStatus = api.Pair (pairingPhrase, userName);
									state = STATE.POLL_FOR_PAIRING;
									break;
								} catch(RequestError err) {
									OnDebugStatus (String.Format ("The pairing phrase was not accepted (reason:{0})", err.Message));
								}
						}
							break;
						};
					case STATE.POLL_FOR_PAIRING: {
					
							pairingStatus = api.GetPairingStatus (pairingStatus.id);
							if(pairingStatus.enabled) {
								OnDebugStatus ("Pairing Complete");
								state = STATE.AUTHENTICATE;
							} else {
								OnDebugStatus ("The pairing has not been authorized by the phone yet.");
							}
							break;
						};
					case STATE.RESET_PAIRING: {
							state = STATE.AUTHENTICATE;

							break;
						};
					case STATE.NAME_TERMINAL: {
							OnDebugStatus ("Naming Terminal");
							String terminalName = OnPromptUser ("Name Terminal:");
							try {
								api.CreateUserTerminal (userName, terminalName, terminalIdentifier);
							} catch(RequestError e) {
								OnDebugStatus (String.Format ("could not create terminal (reason:{0})", e.Message));
								toopherAuthResult = FAILURE;
								done = true;
							}
							state = STATE.AUTHENTICATE;
							break;
						};
					default: {
							OnDebugStatus (String.Format ("Unknown state {0}", state));
							toopherAuthResult = FAILURE;
							done = true;
							break;
						};
				}
				if(!(done || IsCancelled)) {
					Thread.Sleep(1000);
				}
			}

			if(!IsCancelled) {
				OnDone (toopherAuthResult);
			}
			this.IsRunning = false;
		}

	}
}
