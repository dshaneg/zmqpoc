using System;
using System.Threading;
using ZeroMQ;

namespace HWClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // hello world client

            // create
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                // bind
                requester.Connect("tcp://127.0.0.1:5555");
                var name = _AcceptName();

                while (!string.IsNullOrWhiteSpace(name))
                {
                    Console.Write("Sending {0}...", name);

                    // send
                    requester.Send(new ZFrame(name));

                    // receive
                    using (var reply = requester.ReceiveFrame())
                    {
                        Console.WriteLine("Received {0}", reply.ReadString());
                    }

                    name = _AcceptName();
                }
            }
        }

        private static string _AcceptName()
        {
            Console.Write("Enter a name. <Enter> to quit. ");
            return Console.ReadLine();
        }
    }
}
