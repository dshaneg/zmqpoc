using NetMQ;

namespace RRBroker
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Simple request-reply broker
            //
            // Author: metadings
            //

            // Prepare our context and sockets
            using (var ctx = NetMQContext.Create())
            using (var frontend = ctx.CreateRouterSocket())
            using (var backend = ctx.CreateDealerSocket())
            {
                frontend.Bind("tcp://*:5559");
                backend.Bind("tcp://*:5560");

                var proxy = new Proxy(frontend, backend);
                proxy.Start();
            }
        }
    }
}