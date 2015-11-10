using System;
using NetMQ;

namespace RRClient
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Hello World client
            // Connects REQ socket to tcp://localhost:5559
            // Sends "Hello" to server, expects "World" back
            //
            // Author: metadings
            //

            // Socket to talk to server
            using (var context = NetMQContext.Create())
            using (var requester = context.CreateRequestSocket())
            {
                requester.Connect("tcp://127.0.0.1:5559");

                for (var n = 0; n < 10000; ++n)
                {
                    requester.SendFrame("Hello");

                    var reply = requester.ReceiveFrameString();
                    Console.WriteLine("Hello {0}!", reply);
                }
            }

            Console.ReadLine();
        }
    }
}