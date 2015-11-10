using System;
using NetMQ;

namespace WUServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            //
            // Weather update server
            // Binds PUB socket to tcp://*:5556
            // Publishes random weather updates
            //
            // Author: metadings
            //

            // Prepare our context and publisher
            using (var context = NetMQContext.Create())
            using (var publisher = context.CreatePublisherSocket())
            {
                const string address = "tcp://*:5556";

                Console.WriteLine("I: Publisher.Binding on {0}", address);
                publisher.Bind(address);

                // Initialize random number generator
                var rnd = new Random();

                while (true)
                {
                    // Get values that will fool the boss
                    var zipcode = rnd.Next(99999);
                    var temperature = rnd.Next(-55, +45);

                    // Send message to all subscribers
                    publisher.SendFrame(string.Format("{0:D5} {1}", zipcode, temperature));
                }
            }
        }
    }
}
