using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToopherRdpConfigManager {
	public interface ILoggableJob {
		Action<String> Log { get; set; }
		Action Done { get; set; }
		string Name { get; }
		bool Running { get; }
		void Run ();
		void Abort ();
	}

	public class AsyncLoggableJob : ILoggableJob {
		Action<string> ILoggableJob.Log {
			get {
				return job.Log;
			}
			set {
				job.Log = value;
			}
		}

		public Action Done {
			get {
				return job.Done;
			}
			set {
				job.Done = value;
			}
		}

		public string Name {
			get { return job.Name; }
		}

		public bool Running {
			get { return job.Running; }
		}
		ILoggableJob job;
		public AsyncLoggableJob (ILoggableJob job) {
			this.job = job;
		}


		Task t;
		public void Run () {
			t = new Task (job.Run);
			t.Start ();
			
		}

		public void Abort () {
			job.Abort ();
			t.Wait ();
		}
	}

	public class TestApiJob : ILoggableJob {
		public Action<String> Log { get; set; }
		public Action Done { get; set; }
		public String Name {
			get {
				return "Testing Toopher API Credentials...";
			}
		}

		Toopher.ToopherAPI api;
		public TestApiJob (Toopher.ToopherAPI api) {
			this.api = api;
		}

		public bool Running { get; private set; }

		private void testPassed () {
			Log ("Connectivity Test Successful!");
		}
		public void Run () {
			Running = true;
			try {
				Log ("Testing API Connectivity");
				IDictionary<string, object> d = api.SearchForUser ("vqnvq743q3ghahdfas3fvaaaw3");
				testPassed ();
			} catch (Toopher.RequestError e) {
				if(e.Message == "No users with name = vqnvq743q3ghahdfas3fvaaaw3") {
					testPassed ();
				} else {
					Log ("Connectivity Test Failed.");
					Log ("Error Type   : " + e.GetType ().Name);
					Log ("Error Message: " + e.Message);
				}
			} finally {
				Running = false;
				Done ();
			}
		}

		public void Abort () {
			throw new NotImplementedException ();
		}

		
	}
}
