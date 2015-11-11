using System;
using System.Diagnostics;
using NetMQ;

namespace TaskSinkKill
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Task sink - design 2
            // Adds pub-sub flow to send kill signal to workers
            //
            // Author: metadings
            //

            // Socket to receive messages on and
            // Socket for worker control
            using (var context = NetMQContext.Create())
            using (var receiver = context.CreatePullSocket())
            using (var controller = context.CreatePublisherSocket())
            {
                receiver.Bind("tcp://*:5558");
                controller.Bind("tcp://*:5559");


                // Wait for start of batch
                receiver.ReceiveFrameBytes();

                // Start our clock now
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Process 100 confirmations
                for (var i = 0; i < 100; ++i)
                {
                    receiver.ReceiveFrameBytes();

                    Console.Write((i/10)*10 == i ? ":" : ".");
                }

                stopwatch.Stop();
                Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);

                Console.WriteLine("<Enter> to kill workers.");
                Console.ReadLine();

                // Send kill signal to workers
                controller.SendFrame("KILL");
            }
        }
    }
}