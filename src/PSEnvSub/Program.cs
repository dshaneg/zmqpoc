using System;
using System.Linq;
using ZeroMQ;

namespace PSEnvSub
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            //
            // Pubsub envelope subscriber
            //
            // Author: metadings
            //

            string topic;

            if (args == null || !args.Any())
            {
                topic = string.Empty;
                Console.WriteLine("Not filtering messages.");
            }
            else
            {
                topic = args[0];
                Console.WriteLine("Filtering messages by topic '{0}'.", topic);
            }

            // Prepare our context and subscriber
            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                subscriber.Connect("tcp://127.0.0.1:5563");
                subscriber.Subscribe(topic);

                while (true)
                {
                    using (ZMessage message = subscriber.ReceiveMessage())
                    {
                        // Read envelope with address
                        string address = message[0].ReadString();

                        // Read message contents
                        string contents = message[1].ReadString();

                        Console.WriteLine("[{0}] {1}", address, contents);
                    }
                }
            }
        }
    }
}