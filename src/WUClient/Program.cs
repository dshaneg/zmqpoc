using System;
using NetMQ;

namespace WUClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            //
            // Weather update client
            // Connects SUB socket to tcp://localhost:5556
            // Collects weather updates and finds avg temp in zipcode
            //
            // Author: metadings
            //

            if (args.Length < 2)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} [ZipCode] [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    ZipCode   The zip code to subscribe. Default is 72622 Nürtingen");
                Console.WriteLine("    Endpoint  Where WUClient should connect to.");
                Console.WriteLine("              Default is tcp://127.0.0.1:5556");
                Console.WriteLine();
                args = args.Length < 1 
                    ? new[] { "72622", "tcp://127.0.0.1:5556" } 
                    : new[] { args[0], "tcp://127.0.0.1:5556" };
            }

            // Socket to talk to server
            using (var context = NetMQContext.Create())
            using (var subscriber = context.CreateSubscriberSocket())
            {
                var connectTo = args[1];
                Console.WriteLine("I: Connecting to {0}...", connectTo);
                subscriber.Connect(connectTo);

                // Subscribe to zipcode
                var zipCode = args[0];
                Console.WriteLine("I: Subscribing to zip code {0}...", zipCode);
                subscriber.Subscribe(zipCode);

                // Process 10 updates
                //int i = 0;
                //long total_temperature = 0;
                //for (; i < 20; ++i)
                while(true)
                {
                    var reply = subscriber.ReceiveFrameString();

                    Console.WriteLine(reply);
                    //total_temperature += Convert.ToInt64(reply.Split(' ')[1]);
                }

                //Console.WriteLine("Average temperature for zipcode '{0}' was {1}°", zipCode, (total_temperature / i));
                //Console.ReadLine();
            }
        }
    }
}
