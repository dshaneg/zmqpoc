using System;
using System.Threading;
using NetMQ;

namespace TaskWorkKillable
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Task worker - design 2
            // Adds pub-sub flow to receive and respond to kill signal
            //
            // Author: metadings
            //

            // Socket to receive messages on,
            // Socket to send messages to and
            // Socket for control input
            using (var context = NetMQContext.Create())
            using (var receiver = context.CreatePullSocket())
            using (var sender = context.CreatePushSocket())
            using (var controller = context.CreateSubscriberSocket())
            {
                receiver.Connect("tcp://127.0.0.1:5557");
                sender.Connect("tcp://127.0.0.1:5558");
                controller.Connect("tcp://127.0.0.1:5559");

                controller.SubscribeToAnyTopic();

                var poller = new Poller();
                poller.AddSocket(receiver);
                poller.AddSocket(controller);

                receiver.ReceiveReady += (o, eventArgs) =>
                {
                    var workload = int.Parse(eventArgs.Socket.ReceiveFrameString());
                    
                    Console.WriteLine("{0}.", workload); // Show progress

                    Thread.Sleep(workload); // Do the work

                    sender.SendFrame(new byte[0]); // Send results to sink                
                };

                controller.ReceiveReady += (o, eventArgs) =>
                {
                    if (eventArgs.Socket.ReceiveFrameString() == "KILL");
                        poller.Cancel();
                };

                poller.PollTillCancelled();
            }
        }
    }
}