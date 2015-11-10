using System;
using System.Diagnostics;
using NetMQ;

namespace TaskSink
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Task sink
            // Binds PULL socket to tcp://localhost:5558
            // Collects results from workers via that socket
            //
            // Author: metadings
            //

            // Prepare our context and socket
            using (var context = NetMQContext.Create())
            using (var sink = context.CreatePullSocket())
            {
                sink.Bind("tcp://*:5558");

                // Wait for start of batch
                sink.ReceiveFrameBytes();

                // Start our clock now
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Process 100 confirmations
                for (var i = 0; i < 100; ++i)
                {
                    sink.ReceiveFrameBytes();

                    Console.Write((i/10)*10 == i ? ":" : ".");
                }

                // Calculate and report duration of batch
                stopwatch.Stop();
                Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
                Console.ReadLine();
            }
        }
    }
}