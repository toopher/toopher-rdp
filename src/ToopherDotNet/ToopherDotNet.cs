using System;
using OAuth;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Runtime.Serialization;
using SimpleJson;
using System.IO;
using System.Linq;
using System.Collections;

namespace Toopher
{
	public class ToopherAPI
	{



		public const string VERSION = "1.1.0";
		public const string DEFAULT_BASE_URL = "https://api.toopher.com/v1/";

		string consumerKey;
		string consumerSecret;
		string baseUrl;
		Type webClientProxyType;

		// Create the ToopherAPI object tied to your requester credentials
		// 
		// Credentials are available on https://dev.toopher.com
		/// <summary>
		/// Create a new instance of the ToopherAPI client tied to your requester
		/// credentials.  Credentials are available at https://dev.toopher.com
		/// </summary>
		/// <param name="consumerKey">OAuth Consumer Key</param>
		/// <param name="consumerSecret">OAuth Consumer Secret</param>
		/// <param name="baseUrl">Override url for ToopherAPI webservice (default=https://api.toopher.com/v1/) </param>
		/// <param name="webClientType">Override WebClient class for testing purposes</param>
		public ToopherAPI (string consumerKey, string consumerSecret, string baseUrl = null, Type webClientProxyType = null)
		{
			this.consumerKey = consumerKey;
			this.consumerSecret = consumerSecret;
			if (!string.IsNullOrEmpty(baseUrl)) {
				this.baseUrl = baseUrl;
			} else {
				this.baseUrl = ToopherAPI.DEFAULT_BASE_URL;
			}
			if (webClientProxyType != null) {
				this.webClientProxyType = webClientProxyType;
			} else {
				this.webClientProxyType = typeof(WebClientProxy);
			}
		}

		// Pair your requester with a user's Toopher application
		//
		// Must provide a pairing request (generated on their phone) and
		// the user's name
		public PairingStatus Pair (string pairingPhrase, string userName, Dictionary<string, string> extras = null)
		{
			string endpoint = "pairings/create";

			NameValueCollection parameters = new NameValueCollection ();
			parameters.Add ("pairing_phrase", pairingPhrase);
			parameters.Add ("user_name", userName);

			if (extras != null) {
				foreach (KeyValuePair<string, string> kvp in extras) {
					parameters.Add (kvp.Key, kvp.Value);
				}
			}

			var json = post (endpoint, parameters);
			return new PairingStatus (json);
		}

		// Check on status of a pairing request
		// 
		// Must provide the ID returned when the pairing request was initiated
		public PairingStatus GetPairingStatus (string pairingRequestId)
		{
			string endpoint = String.Format ("pairings/{0}", pairingRequestId);

			var json = get (endpoint);
			return new PairingStatus (json);
		}

		// Authenticate an action with Toopher
		//
		// Provide pairing ID, a name for the terminal (displayed to user) and
		// an option action name (displayed to user) [defaults to "log in"]
		public AuthenticationStatus Authenticate (string pairingId, string terminalName, string actionName = null, Dictionary<string, string> extras = null)
		{
			string endpoint = "authentication_requests/initiate";

			NameValueCollection parameters = new NameValueCollection ();
			parameters.Add ("pairing_id", pairingId);
			parameters.Add ("terminal_name", terminalName);
			if (actionName != null) {
				parameters.Add ("action_name", actionName);
			}
			if (extras != null) {
				foreach (KeyValuePair<string, string> kvp in extras) {
					parameters.Add (kvp.Key, kvp.Value);
				}
			}

			var json = post (endpoint, parameters);
			return new AuthenticationStatus (json);
		}

		/// <summary>
		/// Authenticate an action with Toopher
		/// </summary>
		/// <param name="userName">Name of the user</param>
		/// <param name="terminalIdentifier">Unique terminal identifier for this terminal.  Does not need to be human-readable.</param>
		/// <param name="actionName">Name of the action to authenticate.  default = "Login"</param>
		/// <param name="extras">Dictionary of arbitray key/value pairs to add to the webservice call</param>
		/// <returns>AuthenticationStatus object</returns>
		/// <exception cref="UserDisabledError">Thrown when Toopher Authentication is disabled for the user</exception>
		/// <exception cref="UserUnknownError">Thrown when the user has no active pairings</exception>
		/// <exception cref="TerminalUnknownError">Thrown when the terminal cannot be identified</exception>
		/// <exception cref="PairingDeactivatedError">Thrown when the user has deleted the pairing from their mobile device</exception>
		/// <exception cref="RequestError">Thrown when there is a problem contacting the Toopher API</exception>
		public AuthenticationStatus AuthenticateByUserName (string userName, string terminalIdentifier, string actionName = null, Dictionary<string, string> extras = null)
		{
			if (extras == null) {
				extras = new Dictionary<string, string> ();
			}
			extras["user_name"] = userName;
			extras["terminal_name_extra"] = terminalIdentifier;

			return this.Authenticate (null, null, actionName, extras);
		}

		// Check on status of authentication request
		//
		// Provide authentication request ID returned when authentication request was
		// started.
		public AuthenticationStatus GetAuthenticationStatus (string authenticationRequestId, string otp = null)
		{
			string endpoint = String.Format ("authentication_requests/{0}", authenticationRequestId);

			JsonObject json;
			if (String.IsNullOrEmpty (otp)) {
				json = get (endpoint);
				return new AuthenticationStatus (json);
			} else {
				NameValueCollection parameters = new NameValueCollection ();
				parameters["otp"] = otp;
				json = post (endpoint + "/otp_auth", parameters);
			}
			return new AuthenticationStatus (json);
		}

		/// <summary>
		/// Create a named terminal in the Toopher API for the (userName, terminalIdentifier) combination
		/// </summary>
		/// <param name="userName">Name of the user</param>
		/// <param name="terminalName">User-assigned "friendly" terminal name</param>
		/// <param name="terminalIdentifier">Unique terminal identifier for this terminal.  Does not need to be human-readable.</param>
		/// <exception cref="RequestError">Thrown when there is a problem contacting the Toopher API</exception>
		public void CreateUserTerminal (string userName, string terminalName, string terminalIdentifier)
		{
			string endpoint = "user_terminals/create";
			NameValueCollection parameters = new NameValueCollection();
			parameters["user_name"] = userName;
			parameters["name"] = terminalName;
			parameters["name_extra"] = terminalIdentifier;
			post (endpoint, parameters);
		}

				/// <summary>
		/// Search for a given Toopher user.  Return the user ID if present
		/// </summary>
		/// <param name="userName">Name of the user to modify</param>
		/// <exception cref="RequestError">Thrown when there is a problem contacting the Toopher API</exception>
		public IDictionary<string, object> SearchForUser (string userName) {
			string searchEndpoint = "users";
			NameValueCollection parameters = new NameValueCollection ();
			parameters["name"] = userName;

			JsonArray jArr = getArray (searchEndpoint, parameters);
			if(jArr.Count > 1) {
				throw new RequestError ("Multiple users with name = " + userName);
			}
			if(jArr.Count == 0) {
				throw new RequestError ("No users with name = " + userName);
			}

			return (IDictionary<string, object>)jArr[0];
		}

		public List<PairingStatus> GetUserPairings (string userName) {
			IDictionary<string, object> user = SearchForUser (userName);

			string endpoint = "users/" + (string)user["id"] + "/pairings";
			NameValueCollection parameters = new NameValueCollection ();
			parameters["deactivated"] = "0";
			JsonArray jArr = getArray (endpoint, parameters);
			List<PairingStatus> result = new List<PairingStatus> (jArr.Count ());
			foreach(JsonObject o in jArr) {
				result.Add (new PairingStatus (o));
			}
			return result;
		}

		public PairingStatus DeactivatePairing (string pairingId) {
			string endpoint = "pairings/" + pairingId;
			NameValueCollection parameters = new NameValueCollection ();
			parameters["deactivated"] = "True";
			JsonObject o = post (endpoint, parameters);
			return new PairingStatus (o);
		}

		/// <summary>
		/// Enable or Disable Toopher Authentication for an individual user.  If the user is
		/// disabled, future attempts to authenticate the user with Toopher will return
		/// a UserDisabledError
		/// </summary>
		/// <param name="userName">Name of the user to modify</param>
		/// <param name="toopherEnabled">True if the user should be authenticated with Toopher</param>
		/// <exception cref="RequestError">Thrown when there is a problem contacting the Toopher API</exception>
		public void SetToopherEnabledForUser (string userName, bool toopherEnabled)
		{
			string userId = (string)(SearchForUser(userName))["id"];

			string updateEndpoint = "users/" + userId;
			NameValueCollection parameters = new NameValueCollection ();
			parameters = new NameValueCollection ();
			parameters["disable_toopher_auth"] = toopherEnabled ? "false" : "true";
			post (updateEndpoint, parameters);
		}

		public string GetPairingResetLink (string pairingId, string securityQuestion = null, string securityAnswer = null)
		{
			string endpoint = "pairings/" + pairingId + "/generate_reset_link";
			NameValueCollection parameters = new NameValueCollection ();
			parameters["security_question"] = securityQuestion;
			parameters["security_answer"] = securityAnswer;

			JsonObject pairingResetLink = post (endpoint, parameters);
			return (string)pairingResetLink["url"];
		}

		private object request (string method, string endpoint, NameValueCollection parameters = null)
		{
			// Normalize method string
			method = method.ToUpper ();

			// Build an empty collection for parameters (if necessary)
			if (parameters == null) {
				parameters = new NameValueCollection ();
			}

			// can't have null parameters, or oauth signing will barf
			foreach (String key in parameters.AllKeys){
				if (parameters[key] == null) {
					parameters[key] = "";
				}
			}

			var client = OAuthRequest.ForRequestToken (this.consumerKey, this.consumerSecret);
			client.RequestUrl = this.baseUrl + endpoint;
			client.Method = method;

			string auth = client.GetAuthorizationHeader (parameters);
			// FIXME: OAuth library puts extraneous comma at end, workaround: remove it if present
			auth = auth.TrimEnd (new char[] { ',' });

			using (WebClientProxy wClient = (WebClientProxy)Activator.CreateInstance(webClientProxyType)) {
				wClient.Headers.Add ("Authorization", auth);
				wClient.Headers.Add ("User-Agent",
					string.Format ("Toopher-DotNet/{0} (DotNet {1})", VERSION, Environment.Version.ToString ()));
				if (parameters.Count > 0) {
					wClient.QueryString = parameters;
				}

				string response;
				try {
					if (method.Equals ("POST")) {
						var responseArray = wClient.UploadValues (client.RequestUrl, client.Method, parameters);
						response = Encoding.UTF8.GetString (responseArray);
					} else {
						response = wClient.DownloadString (client.RequestUrl);
					}
				} catch (WebException wex) {
					IHttpWebResponse httpResp = HttpWebResponseWrapper.create(wex.Response);
					string error_message;
					using (Stream stream = httpResp.GetResponseStream ()) {
						StreamReader reader = new StreamReader (stream, Encoding.UTF8);
						error_message = reader.ReadToEnd ();
					}

					String statusLine = httpResp.StatusCode.ToString () + " : " + httpResp.StatusDescription;

					if (String.IsNullOrEmpty (error_message)) {
						throw new RequestError (statusLine);
					} else {

						try {
							// Attempt to parse JSON response
							var json = (JsonObject)SimpleJson.SimpleJson.DeserializeObject (error_message);
							parseRequestError (json);
						} catch (RequestError e) {
							throw e;
						} catch (Exception) {
							throw new RequestError (statusLine + " : " + error_message);
						}
					}

					throw new RequestError (error_message, wex);
				}
			
				try {
					return SimpleJson.SimpleJson.DeserializeObject (response);
				} catch (Exception ex) {
					throw new RequestError ("Could not parse response", ex);
				}
			}

		}

		
		private JsonObject get (string endpoint, NameValueCollection parameters = null)
		{
			return (JsonObject)request ("GET", endpoint, parameters);
		}
		private JsonArray getArray (string endpoint, NameValueCollection parameters = null)
		{
			return (JsonArray)request ("GET", endpoint, parameters);
		}

		private JsonObject post (string endpoint, NameValueCollection parameters = null)
		{
			return (JsonObject) request ("POST", endpoint, parameters);
		}

		private void parseRequestError (JsonObject err)
		{
			long errCode = (long)err["error_code"];
			string errMessage = (string)err["error_message"];
			if (errCode == UserDisabledError.ERROR_CODE) {
				throw new UserDisabledError ();
			} else if (errCode == UserUnknownError.ERROR_CODE) {
				throw new UserUnknownError ();
			} else if (errCode == TerminalUnknownError.ERROR_CODE) {
				throw new TerminalUnknownError ();
			} else {
				if (errMessage.ToLower ().Contains ("pairing has been deactivated")
					|| errMessage.ToLower ().Contains ("pairing has not been authorized")) {
					throw new PairingDeactivatedError ();
				} else {
					throw new RequestError (errMessage);
				}
			}

		}
	}

	// Status information for a pairing request
	public class PairingStatus
	{
		private IDictionary<string, Object> _dict;

		public object this[string key]
		{
			get
			{
				return _dict[key];
			}
		}

		public string id
		{
			get;
			private set;
		}
		public string userId
		{
			get;
			private set;
		}
		public string userName
		{
			get;
			private set;
		}
		public bool pending
		{
			get;
			private set;
		}
		public bool enabled
		{
			get;
			private set;
		}

		public override string ToString ()
		{
			return string.Format ("[PairingStatus: id={0}; userId={1}; userName={2}, enabled={3}, pending={4}]", id, userId, userName, enabled, pending);
		}

		public PairingStatus (IDictionary<string, object> _dict)
		{
			try {
				this._dict = _dict;
				this.id = (string)_dict["id"];
				this.pending = (bool)_dict["pending"];
				this.enabled = (bool)_dict["enabled"];

				var user = (JsonObject)_dict["user"];
				this.userId = (string)user["id"];
				this.userName = (string)user["name"];
			} catch (Exception ex) {
				throw new RequestError ("Could not parse pairing status from response", ex);
			}
		}

	}

	// Status information for an authentication request
	public class AuthenticationStatus
	{
		private IDictionary<string, object> _dict;
		public object this[string key]
		{
			get
			{
				return _dict[key];
			}
		}

		public string id
		{
			get;
			private set;
		}
		public bool pending
		{
			get;
			private set;
		}
		public bool granted
		{
			get;
			private set;
		}
		public bool automated
		{
			get;
			private set;
		}
		public string reason
		{
			get;
			private set;
		}
		public string terminalId
		{
			get;
			private set;
		}
		public string terminalName
		{
			get;
			private set;
		}

		public override string ToString ()
		{
			return string.Format ("[AuthenticationStatus: id={0}; pending={1}; granted={2}; automated={3}; reason={4}; terminalId={5}; terminalName={6}]", id, pending, granted, automated, reason, terminalId, terminalName);
		}

		public AuthenticationStatus (IDictionary<string, object> _dict)
		{
			this._dict = _dict;
			try {
				// validate that the json has the minimum keys we need
				this.id = (string)_dict["id"];
				this.pending = (bool)_dict["pending"];
				this.granted = (bool)_dict["granted"];
				this.automated = (bool)_dict["automated"];
				this.reason = (string)_dict["reason"];

				var terminal = (JsonObject)_dict["terminal"];
				this.terminalId = (string)terminal["id"];
				terminalName = (string)terminal["name"];
			} catch (Exception ex) {
				throw new RequestError ("Could not parse authentication status from response", ex);
			}
		}
	}

	// An exception class used to indicate an error in a request
	public class RequestError : System.ApplicationException
	{
		public RequestError () : base () { }
		public RequestError (string message) : base (message) { }
		public RequestError (string message, System.Exception inner) : base (message, inner) { }
	}

	/// <summary>
	/// Thrown when a requester attempts to authenticate a user who has been disabled
	/// </summary>
	public class UserDisabledError : RequestError
	{
		static public int ERROR_CODE = 704;
	}

	/// <summary>
	/// Thrown when there is no active pairing for the user.
	/// Requester should respond by guiding the user through the pairing process,
	/// then re-authenticating
	/// </summary>
	public class UserUnknownError : RequestError
	{
		static public int ERROR_CODE = 705;
	}

	/// <summary>
	/// Thrown when Toopher API encounters an unknown (user, requesterTerminalIdentifier) tuple.
	/// Requester should respond by assigning a friendly name to the terminal
	/// </summary>
	public class TerminalUnknownError : RequestError
	{
		static public int ERROR_CODE = 706;
	}

	/// <summary>
	/// Thrown when the user has deleted the pairing on their mobile device.
	/// Requester should prompt user to re-pair their account
	/// </summary>
	public class PairingDeactivatedError : RequestError
	{
	}

	/// <summary>
	/// Design-For-Testability shims from here on down...
	/// </summary>
	
	public class WebClientProxy : IDisposable
	{
		WebClient _theClient = new WebClient ();
		public WebHeaderCollection Headers
		{
			get { return _theClient.Headers; }
			set { _theClient.Headers = value; }
		}
		public NameValueCollection QueryString
		{
			get { return _theClient.QueryString; }
			set { _theClient.QueryString = value; }
		}
		virtual public byte[] UploadValues (string requestUri, string method, NameValueCollection parameters)
		{
			return _theClient.UploadValues (requestUri, method, parameters);
		}
		virtual public string DownloadString (string requestUri)
		{
			return _theClient.DownloadString (requestUri);
		}

		public void Dispose ()
		{
			if (_theClient != null) {
				_theClient.Dispose ();
				_theClient = null;
			}
		}
	}

	public interface IHttpWebResponse
	{
		Stream GetResponseStream ();
		HttpStatusCode StatusCode { get; }
		string StatusDescription { get; }
	}

	class HttpWebResponseWrapper : IHttpWebResponse
	{
		private HttpWebResponse _wrapped;
		public HttpWebResponseWrapper (HttpWebResponse wrapped)
		{
			this._wrapped = wrapped;
		}
		static public IHttpWebResponse create (object response)
		{
			//if it already implements IHttpWebResponse, return it directory
			if(typeof(IHttpWebResponse).IsAssignableFrom(response.GetType())){
				return (IHttpWebResponse)response;
			} else if (typeof(HttpWebResponse).IsAssignableFrom(response.GetType())) {
				return new HttpWebResponseWrapper((HttpWebResponse)response);
			} else {
				throw new NotImplementedException("Don't know how to transmute " + response.GetType().ToString() + " into IHttpWebResponse");
			}
		}

		public Stream GetResponseStream ()
		{
			return _wrapped.GetResponseStream();
		}

		public HttpStatusCode StatusCode
		{
			get { return _wrapped.StatusCode; }
		}

		public string StatusDescription
		{
			get { return _wrapped.StatusDescription; }
		}
	}

}

