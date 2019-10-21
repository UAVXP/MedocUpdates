using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.NetworkInformation;

namespace MedocUpdates
{
	class MedocNetwork
	{
		Ping ping;
		bool bNetworkAvailable;
		//IPHostEntry hostentry;
		frmMain parent;

		public event EventHandler NetworkIsUp = delegate { };

		public MedocNetwork(frmMain parent)
		{
			this.parent = parent;

			//NetworkInterface.GetIsNetworkAvailable()
			NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

			ping = new Ping();
			bNetworkAvailable = false;

			//hostentry = new IPHostEntry();
		}

		public void Run()
		{
			parent.Invoke(parent.NetworkCheckingDelegate);
		}

		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			Console.WriteLine("Network is " + (e.IsAvailable ? "available" : "unavailable"));
			bNetworkAvailable = e.IsAvailable;
		}

		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
			
			//if(!bNetworkAvailable)
			if(!NetworkInterface.GetIsNetworkAvailable())
				return;

			//hostentry = Dns.GetHostEntry("https://google.com/"); // System.Net.Sockets.SocketException
			//if (hostentry.AddressList.Length <= 0)
			//	return;

			/*
				try
				{
					PingReply reply = ping.Send("https://google.com");
					//if(reply.Status == IPStatus.)
					Console.WriteLine("Network address changed, ping status is " + reply.Status);
				}
				catch(System.Net.Sockets.SocketException ex)
				{
					Log.Write(LogLevel.EXPERT, "MedocNetwork: Socket is unavailable\r\n" + ex.Message);
				}
				catch(Exception ex)
				{
					//Log.Write(LogLevel.EXPERT, "MedocNetwork: Socket is unavailable\r\n" + ex.Message);
				}
			*/
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
			NetworkIsUp.Invoke(this, new EventArgs());
		}
	}
}
