using System;
using NetMQ;

namespace HWClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // hello world client

            // create
            using (var context = NetMQContext.Create())
            using (var requester = context.CreateRequestSocket())
            {
                // bind
                requester.Connect("tcp://127.0.0.1:5555");
                var name = _AcceptName();

                while (!string.IsNullOrWhiteSpace(name))
                {
                    Console.Write("Sending {0}...", name);

                    // send
                    requester.SendFrame(name);

                    // receive
                    var reply = requester.ReceiveFrameString();
                    
                    Console.WriteLine("Received {0}", reply);

                    name = _AcceptName();
                }
            }
        }

        private static string _AcceptName()
        {
            Console.Write("Enter a name. <Enter> to quit. ");
            return Console.ReadLine();
        }
    }
}
