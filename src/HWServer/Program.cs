using System;
using System.Threading;
using NetMQ;

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
                args = new[] { "Hello" };
            }

            var greeting = args[0];

            // create

            using (var context = NetMQContext.Create())
            using (var responder = context.CreateResponseSocket())
            {
                // bind
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    // receive
                    var name = responder.ReceiveFrameString();

                    Console.WriteLine("Received {0}", name);

                    // do some work
                    Thread.Sleep(1);

                    // send
                    responder.SendFrame(string.Format("{0} {1}!", greeting, name));
                }
            }
        }
    }
}
