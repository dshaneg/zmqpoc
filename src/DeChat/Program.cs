using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZeroMQ;

namespace DeChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = _ParseCommandLineArgs(args);
            if (arguments.HasError)
            {
                Console.WriteLine(arguments.ErrorMessage);
                return;
            }

            using (var context = new ZContext())
            {
                new Thread(() => _Subscribe(context, arguments.IpAddress)).Start();
                
                _Publish(context, arguments.Alias);
            }
        }

        private class Args
        {
            public Args(string errorMessage)
            {
                HasError = true;
                ErrorMessage = errorMessage;
            }

            public Args(IPAddress ipAddress, string alias)
            {
                IpAddress = ipAddress;
                Alias = alias;
            }

            public IPAddress IpAddress { get; private set; }
            public string Alias { get; private set; }

            public bool HasError { get; private set; }

            public string ErrorMessage { get; private set; }
        }

        private static Args _ParseCommandLineArgs(string[] argList)
        {
            if (argList.Length != 2)
                return new Args(string.Format("Usage: {0} ipaddress alias", AppDomain.CurrentDomain.FriendlyName));

            IPAddress ipaddress;
            if (!IPAddress.TryParse(argList[0], out ipaddress))
                return new Args(string.Format("Invalid IPv4 address: {0}", argList[1]));

            if (ipaddress.AddressFamily != AddressFamily.InterNetwork)
                return new Args("ipaddress must be IPv4.");

            return new Args(ipaddress, argList[1]);
        }

        private static void _Publish(ZContext context, string alias)
        {
            using (var broadcaster = new ZSocket(context, ZSocketType.PUB))
            {
                const string address = "tcp://*:9000";
                Console.WriteLine("Binding on {0}", address);
                broadcaster.Bind(address);

                while (true)
                {
                    var message = Console.ReadLine();

                    using (var sendFrame = new ZFrame(string.Format("{0}: {1}", alias, message)))
                    {
                        broadcaster.Send(sendFrame);
                    }
                }
            }
        }

        private static void _Subscribe(ZContext context, IPAddress ipaddress)
        {
            // parse out the hostid from the ipaddress
            var networkId = _ExtractNetworkId(ipaddress);

            using (var listener = new ZSocket(context, ZSocketType.SUB))
            {
                for (var hostId = 1; hostId < 255; hostId++)
                {
                    listener.Connect(string.Format("tcp://{0}.{1}:9000", networkId, hostId));
                }

                listener.Subscribe("");

                while (true)
                {
                    using (var frame = listener.ReceiveFrame())
                    {
                        Console.WriteLine(frame.ReadString());
                    }
                }
            }
        }

        private static string _ExtractNetworkId(IPAddress ipaddress)
        {
            var fam = ipaddress.AddressFamily == AddressFamily.InterNetwork;
            var parts = ipaddress.MapToIPv4().ToString().Split(new[] {'.'}).Take(3);
            var networkId = string.Join(".", parts);
            return networkId;
        }
    }
}
