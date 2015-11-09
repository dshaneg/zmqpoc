using System;
using System.Diagnostics;
using ZeroMQ;

namespace TaskSink
{
    static partial class Program
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
            using (var context = new ZContext())
            using (var sink = new ZSocket(context, ZSocketType.PULL))
            {
                sink.Bind("tcp://*:5558");

                // Wait for start of batch
                sink.ReceiveFrame();

                // Start our clock now
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Process 100 confirmations
                for (int i = 0; i < 100; ++i)
                {
                    sink.ReceiveFrame();

                    if ((i / 10) * 10 == i)
                        Console.Write(":");
                    else
                        Console.Write(".");
                }

                // Calculate and report duration of batch
                stopwatch.Stop();
                Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
                Console.ReadLine();
            }
        }
    }
}