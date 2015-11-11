using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NetMQ;

namespace WUProxy
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            //
            // Weather proxy device
            //
            // Author: metadings
            //

            using (var context = NetMQContext.Create())
            using (var frontend = context.CreateXSubscriberSocket())
            using (var backend = context.CreateXPublisherSocket())
            {
                // Frontend is where the weather server sits
                const string localhost = "tcp://127.0.0.1:5556";
                Console.WriteLine("I: Connecting to {0}", localhost);
                frontend.Connect(localhost);

                // Backend is our public endpoint for subscribers
                foreach (var address in WUProxy_GetPublicIPs())
                {
                    var tcpAddress = string.Format("tcp://{0}:8100", address);
                    Console.WriteLine("I: Binding on {0}", tcpAddress);
                    backend.Bind(tcpAddress);

                    var epgmAddress = string.Format("epgm://{0};239.192.1.1:8100", address);
                    Console.WriteLine("I: Binding on {0}", epgmAddress);
                    backend.Bind(epgmAddress);
                }

                backend.SendFrame(new byte[] { 0x1 });

                // Run the proxy until the user interrupts us
                var proxy = new Proxy(frontend, backend);
                proxy.Start();
            }
        }

        static IEnumerable<IPAddress> WUProxy_GetPublicIPs()
        {
            var list = new List<IPAddress>();
            var ifaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in ifaces)
            {
                if (iface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                if (iface.OperationalStatus != OperationalStatus.Up)
                    continue;

                var props = iface.GetIPProperties();
                var addresses = props.UnicastAddresses;
                foreach (var address in addresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        list.Add(address.Address);
                    // if (address.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    //    list.Add(address.Address);
                }
            }
            return list;
        }
    }
}