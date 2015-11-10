using System;
using System.Globalization;
using NetMQ;

namespace TaskVent
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Task ventilator
            // Binds PUSH socket to tcp://localhost:5557
            // Sends batch of tasks to workers via that socket
            //
            // Author: metadings
            //

            // Socket to send messages on and
            // Socket to send start of batch message on
            using (var context = NetMQContext.Create())
            using (var sender = context.CreatePushSocket())
            using (var sink = context.CreatePushSocket())
            {
                sender.Bind("tcp://*:5557");
                sink.Connect("tcp://127.0.0.1:5558");

                Console.WriteLine("Press ENTER when the workers are ready…");
                Console.ReadKey(true);
                Console.WriteLine("Sending tasks to workers…");

                // The first message is "0" and signals start of batch
                sink.SendFrame("0");

                // Initialize random number generator
                var rnd = new Random();

                // Send 100 tasks
                long totalMsec = 0;    // Total expected cost in msecs
                for (var i = 0; i < 100; ++i)
                {
                    // Random workload from 1 to 100msecs
                    var workload = rnd.Next(100) + 1;
                    totalMsec += workload;

                    Console.WriteLine("{0}", workload);
                    sender.SendFrame(workload.ToString(CultureInfo.InvariantCulture));
                }

                Console.WriteLine("Total expected cost: {0} ms", totalMsec);
                Console.ReadLine();
            }
        }
    }
}