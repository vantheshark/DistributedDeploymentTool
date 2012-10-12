using System;
using System.Threading;

namespace DistributedDeployment.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Simulate a long running service
            var svc = new LongRunningService();
            Server.Start<ISignalListener>(svc, "LongRunningService");



            Console.WriteLine("Open cmd promt, type \"DD -s LongRunningService -w 10\" without quotes to exit this console app");
            Console.ReadKey();
        }
    }

    public class LongRunningService : ISignalListener
    {
        public void Exit()
        {
            Console.WriteLine("Received exit signal, wait 2 seconds before exiting");
            Thread.Sleep(2000);
            Console.WriteLine("Service is exiting");
        }
    }
}
