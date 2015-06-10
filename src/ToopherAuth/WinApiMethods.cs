using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToopherAuth {
	public class WinApiMethods {
		public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
		public static int WTS_CURRENT_SESSION = -1;
		
		public enum WTS_INFO_CLASS {
			WTSInitialProgram,
			WTSApplicationName,
			WTSWorkingDirectory,
			WTSOEMId,
			WTSSessionId,
			WTSUserName,
			WTSWinStationName,
			WTSDomainName,
			WTSConnectState,
			WTSClientBuildNumber,
			WTSClientName,
			WTSClientDirectory,
			WTSClientProductId,
			WTSClientHardwareId,
			WTSClientAddress,
			WTSClientDisplay,
			WTSClientProtocolType
		}
		private static String s = "";
		private static void addBytesToStream (Stream s, byte[] b) {
			s.Write (b, 0, b.Length);
		}

		public enum CLIENT_PROTO_TYPE : short {
			Console = 0,
			Legacy = 1,
			RDP = 2
		};

		public enum CLIENT_ADDR_TYPE : int {
			AF_UNSPEC = 0,
			AF_UNIX = 1,
			AF_INET = 2,
			AF_IMPLINK = 3,
			AF_INET6 = 23
		}

		private static byte[] getClientAddrBytes () {
			byte[] buf = getTerminalInfo (WTS_INFO_CLASS.WTSClientAddress);
			switch((int)BitConverter.ToUInt32 (buf, 0)) {
				case (int)CLIENT_ADDR_TYPE.AF_INET: return buf.Skip (6).Take (4).ToArray ();
				case (int)CLIENT_ADDR_TYPE.AF_INET6: return buf;
				default: return buf.Take (4).ToArray ();
			}

		}
		public static string buildTerminalIdentifier () {
			var idStream = new MemoryStream ();
			UInt16 proto = BitConverter.ToUInt16 (getTerminalInfo (WTS_INFO_CLASS.WTSClientProtocolType), 0);
			if(proto == (short)CLIENT_PROTO_TYPE.RDP) {
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSWorkingDirectory));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSOEMId));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientBuildNumber));
				addBytesToStream (idStream, getClientAddrBytes ());
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientName));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientDirectory));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientProductId));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientHardwareId));
				addBytesToStream (idStream, getTerminalInfo (WTS_INFO_CLASS.WTSClientProtocolType));
			} else {
				// use system CPU Processor ID
				System.Management.ManagementClass theClass = new System.Management.ManagementClass ("Win32_Processor");
				System.Management.ManagementObjectCollection theCollectionOfResults = theClass.GetInstances ();
				String s = String.Empty;
				foreach(System.Management.ManagementObject currentResult in theCollectionOfResults) {
					s += currentResult["ProcessorID"].ToString ();
				}
				byte[] b = System.Text.Encoding.Unicode.GetBytes (s);
				idStream.Write (b, 0, b.Length);
			}

			byte[] result;

			using(SHA256 shaM = new SHA256CryptoServiceProvider ()) {
				result = shaM.ComputeHash (idStream.ToArray ());
			}
			return Convert.ToBase64String (result);
		}


		[DllImport ("Wtsapi32.dll")]
		public static extern bool WTSQuerySessionInformation (System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

		[DllImport ("wtsapi32.dll")]
		public static extern void WTSFreeMemory (IntPtr pMemory);

		public static byte[] getTerminalInfo (WTS_INFO_CLASS wtsInfoClass) {
			IntPtr pBuf = IntPtr.Zero;
			uint pBytesReturned;
			WTSQuerySessionInformation (WTS_CURRENT_SERVER_HANDLE,
				WTS_CURRENT_SESSION,
				wtsInfoClass,
				out pBuf,
				out pBytesReturned);

			byte[] buf = new byte[pBytesReturned];
			Marshal.Copy (pBuf, buf, 0, (int)pBytesReturned);

			WTSFreeMemory (pBuf);
			return buf;
		}

		public static String getTerminalInfoString (WTS_INFO_CLASS wtsInfoClass) {
			IntPtr pBuf = IntPtr.Zero;
			uint pBytesReturned;
			WTSQuerySessionInformation (WTS_CURRENT_SERVER_HANDLE,
				WTS_CURRENT_SESSION,
				wtsInfoClass,
				out pBuf,
				out pBytesReturned);

			String result = Marshal.PtrToStringAnsi (pBuf);

			WTSFreeMemory (pBuf);

			return result;
		}
	}
}
