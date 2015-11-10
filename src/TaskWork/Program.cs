using System;
using System.Threading;
using NetMQ;

namespace TaskWork
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Task worker
            // Connects PULL socket to tcp://localhost:5557
            // Collects workloads from ventilator via that socket
            // Connects PUSH socket to tcp://localhost:5558
            // Sends results to sink via that socket
            //
            // Author: metadings
            //

            // Socket to receive messages on and
            // Socket to send messages to
            using (var context = NetMQContext.Create())
            using (var receiver = context.CreatePullSocket())
            using (var sink = context.CreatePushSocket())
            {
                receiver.Connect("tcp://127.0.0.1:5557");
                sink.Connect("tcp://127.0.0.1:5558");

                // Process tasks forever
                while (true)
                {
                    var workMessage = receiver.ReceiveFrameString();
                    var workload = int.Parse(workMessage);
                    Console.WriteLine("{0}.", workload);    // Show progress

                    Thread.Sleep(workload);    // Do the work

                    sink.SendFrameEmpty();    // Send results to sink
                }
            }
        }
    }
}