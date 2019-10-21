using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MedocUpdates
{
	class MedocNetwork
	{
		frmMain parent;

		public event EventHandler NetworkIsUp = delegate { };

		public MedocNetwork(frmMain parent)
		{
			this.parent = parent;

			//NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
		}

		public void Run()
		{
			parent.Invoke(parent.NetworkCheckingDelegate);
		}

		// https://stackoverflow.com/questions/13634868/get-the-default-gateway
		public static IPAddress GetDefaultGateway()
		{
			return NetworkInterface
				.GetAllNetworkInterfaces()
				.Where(n => n.OperationalStatus == OperationalStatus.Up)
				.Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
				.SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
				.Select(g => g?.Address)
				.Where(a => a != null)
				.Where(a => a.AddressFamily == AddressFamily.InterNetwork)
				.Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
				.FirstOrDefault();
		}

		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
			if(!NetworkInterface.GetIsNetworkAvailable())
			{
				Log.Write(LogLevel.NORMAL, "MedocNetwork: Network connection is down");
				return;
			}

			// NOTE: google.com is not that reliable as the network gateway is
			//IPHostEntry hostentry = Dns.GetHostEntry("https://google.com/"); // System.Net.Sockets.SocketException
			//if (hostentry.AddressList.Length <= 0)
			//	return;

		/*
			NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface iface in interfaces)
			{
				IPInterfaceStatistics stats = iface.GetIPStatistics();
				long received = stats.BytesReceived;

				//if(iface.OperationalStatus == OperationalStatus.Up)
					Console.WriteLine("Interface {0} is {1} now", iface.Name, iface.OperationalStatus);
			}
		*/

			try
			{
				if (GetDefaultGateway() != null)
				{
					Log.Write(LogLevel.EXPERT, "MedocNetwork: Invoking main frame version update");
					NetworkIsUp.Invoke(this, new EventArgs());
				}
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.EXPERT, "MedocNetwork: Cannot reach the default gateway address");
			}
		}
	}
}
