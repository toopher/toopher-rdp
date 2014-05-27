using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toopher.Shared.Logging {
	public interface ILog {
		void InfoFormat (string format, params object[] args);
		void DebugFormat (string format, params object[] args);
		void Error (string msg);
		void Debug (string msg);
	}

	public class LogImpl : ILog {
		public void InfoFormat (string format, params object[] args) {
			Console.WriteLine (format, args);
		}
		public void DebugFormat (string format, params object[] args) {
			Console.WriteLine (format, args);
		}
		public void Error (string msg) {
			Console.WriteLine (msg);
		}
		public void Debug (string msg) {
			Console.WriteLine (msg);
		}
	}

	public class LogManager {
		public static void Init () { }
		public static ILog GetLogger (String name) {
			return new LogImpl ();
		}
	}
}
