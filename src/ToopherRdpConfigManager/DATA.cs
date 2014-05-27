using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToopherRdpConfigManager {
	public static class DATA {
		public static string README = @"
Prerequisites:
Windows Server 2008 (or later) OR Windows Vista (or later).
  Windows 2000, XP, and Server 2003 are not supported.
Both 32-bit and 64-bit architectures are supported.

Installation Instructions:
1. Configure your Toopher API Key and Secret in the 'Configuration' tab
1a. Test your configuration button with the 'Test API Connectivity' button
2. Save configuration settings (Configuration -> Save Settings)
3. Install the Toopher Credential Provider (Configuration -> Install)
4. Test it out by opening a Remote Desktop session from a different terminal.

In Case You Get Stuck:
Because this is early software, it is possible that the Credential Provider
may malfunction and lock you out of the server.  To remedy this situation:

1. Reboot server into safe mode to bypass Toopher Credential Provider
2. From the ToopherRDP program directory (usually C:\Program Files\Toopher\ToopherRDP),
   run the following command:

    config uninstall

3. Reboot the server and log in normally


KNOWN ISSUES:
1. Extraneous LoginUI tile is present when authenticating from the machine where Toopher-RDP
   is installed.  Remote users do not see the extra login tile; it is only visible when the
   Toopher-RDP server is being used as a RDP client to another machine.
";
	}
}
