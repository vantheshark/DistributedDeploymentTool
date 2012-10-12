using System;
using System.Collections.Generic;
using DistributedDeployment.Commands;
using DistributedDeployment.NDesk;

namespace DistributedDeployment
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showHelp = false;
            var parsedParams = new Dictionary<string, object>();

            var p = new OptionSet
            {
                {
                    "l|listen|port=", "the {PORT} on which the deployment service is listenning. " +
                    "This must be an integer.",
                    (int v) => parsedParams["port"] = v
                },
                {
                    "w|wait=",
                    "the number of {SECONDS} to wait.\n" +
                    "this must be an integer.",
                    (int v) => parsedParams["waitTime"] = v
                },
                {
                    "f|kill=", "kill the target {PROCESS} if timeout.",
                    v => parsedParams["killIfTimeout"] = v
                },
                {
                    "s|stop=", "the {SERVICE} to send stop signal to." +
                    " This can be anything that unique identify the service",
                    v =>  parsedParams["targetService"] = v
                },
                {
                    "r|e|execute=", "execute a {COMMAND} on remote server",
                    v =>  parsedParams["remoteCommand"] = v
                },
                {
                    "t|target=", "the {ADDRESS} of the remote server in format IP:PORT",
                    v =>  parsedParams["remoteAddress"] = v
                },
                {
                    "k|token|key=", "the {SECURITYTOKEN} to pass to the remote server",
                    v =>  parsedParams["securityToken"] = v
                },
                {
                    "h|help", "show this message and exit",
                    v => showHelp = v != null
                },
            };


            ICommand command = null;
            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("DD: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `DD --help' for more information.");
                return;
            }

            try
            {
                command = CommandFactory.Create(parsedParams);
            }
            catch (Exception)
            {
                showHelp = true;
            }

            if (showHelp || command == null)
            {
                ShowHelp(p);
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }
            Console.WriteLine(command.Execute());
#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void ShowHelp(OptionSet p)
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
