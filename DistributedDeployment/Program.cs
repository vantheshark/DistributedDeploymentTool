using System;
using System.ServiceProcess;
using DistributedDeployment.NDesk;

namespace DistributedDeployment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var svc = new DeploymentService(args);
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(svc);
            }
            else
            {
                svc.Start();
                svc.Stop();
            }
        }

        internal static void ReadKey()
        {
            if(Environment.UserInteractive)
            {
                Console.ReadKey();
            }
        }

        internal static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Distributed Deployment tool");
            Console.WriteLine("Usage: DD [OPTIONS]");
            Console.WriteLine("    [1] Send exit signal to an application");
            Console.WriteLine();

            Console.WriteLine("    [2] Start as a server to listening on remote commands");
            Console.WriteLine();

            Console.WriteLine("    [3] Execute a remote commands. Use as a client for [2]");
            Console.WriteLine();
            Console.WriteLine("Options:");

            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
