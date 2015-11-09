using System;
using System.Threading;
using ZeroMQ;

namespace PSEnvPub
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            //
            // Pubsub envelope publisher
            //
            // Author: metadings
            //

            // Prepare our context and publisher
            using (var context = new ZContext())
            using (var publisher = new ZSocket(context, ZSocketType.PUB))
            {
                publisher.Linger = TimeSpan.Zero;
                publisher.Bind("tcp://*:5563");

                while (true)
                {
                    // Write two messages, each with an envelope and content
                    using (var message = new ZMessage())
                    {
                        message.Add(new ZFrame("A"));
                        message.Add(new ZFrame("Message for topic A."));
                        publisher.Send(message);
                    }
                    using (var message = new ZMessage())
                    {
                        message.Add(new ZFrame("B"));
                        message.Add(new ZFrame("Message for topic B."));
                        publisher.Send(message);
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}