using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace UdpPingMarkI
{
    class Program
    {
        private const int PingPortNumber = 9999;
        private const int PingMsgSize = 1;
        private const int PingIntervalMs = 1000;


        static void Main(string[] args)
        {
            using (var context = new ZContext())
            using (var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                // Ask OS to let us do broadcasts from socket
                udpSocket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.Broadcast, 1);

                // Bind UDP socket to local port so we can receive pings
                udpSocket.Bind(new IPEndPoint(IPAddress.Any, PingPortNumber));

                // We use zmq_poll to wait for activity on the UDP socket, because
                // this function works on non-0MQ file handles. We send a beacon
                // once a second, and we collect and report beacons that come in
                // from other nodes:

                
                //var poller = new z
                //var pollItemsList = new List<ZPollItem>();
                //pollItemsList.Add(new ZPollItem(ZPoll.In));
                //var pollItem = ZPollItem.CreateReceiver();
                //ZMessage message;
                //ZError error;
                //pollItem.ReceiveMessage(udpSocket, out message, out error);
                //pollItem.ReceiveMessage = (ZSocket socket, out ZMessage message, out ZError error) => 

                // Send first ping right away

            }

        }
    }
}
