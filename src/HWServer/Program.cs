using System;
using System.Threading;
using ZeroMQ;

namespace HWServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // hello world server

            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWServer [Greeting]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("   Greeting   A salutation. Default: Hello");
                Console.WriteLine();
                args = new string[] { "Hello" };
            }

            string greeting = args[0];

            // create

            using (var context = new ZContext())
            using (var responder = new ZSocket(context, ZSocketType.REP))
            {
                // bind
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    // receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        var name = request.ReadString();

                        Console.WriteLine("Received {0}", name);

                        // do some work
                        Thread.Sleep(1);

                        // send
                        responder.Send(new ZFrame(string.Format("{0} {1}!", greeting, name)));
                    }
                }
            }
        }
    }
}
