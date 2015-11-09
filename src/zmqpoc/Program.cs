using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;


namespace zmqpoc
{
    class Program
    {
        static void Main(string[] args)
        {
            // hello world server

            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWServer [Name]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("   Name   Your name. Default: World");
                Console.WriteLine();
                args = new string[] {"World"};
            }

            string name = args[0];

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
                        Console.WriteLine("Received {0}", request.ReadString());

                        // do some work
                        Thread.Sleep(1);

                        // send
                        responder.Send(new ZFrame(name));
                    }
                }
            }
        }
    }
}
