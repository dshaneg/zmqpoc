﻿using System;
using ZeroMQ;

namespace RRClient
{
    static partial class Program
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
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                requester.Connect("tcp://127.0.0.1:5559");

                for (int n = 0; n < 10000; ++n)
                {
                    requester.Send(new ZFrame("Hello"));

                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        Console.WriteLine("Hello {0}!", reply.ReadString());
                    }
                }
            }

            Console.ReadLine();
        }
    }
}