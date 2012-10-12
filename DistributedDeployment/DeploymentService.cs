using System;
using System.Collections.Generic;
using System.ServiceProcess;
using DistributedDeployment.Commands;
using DistributedDeployment.NDesk;

namespace DistributedDeployment
{
    public partial class DeploymentService : ServiceBase
    {
        private readonly string[] _args;
        private ICommand _command = null;

        public DeploymentService(string[] args)
        {
            _args = args;
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(_args);
        }

        protected override void OnStart(string[] args)
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
                _command = CommandFactory.Create(parsedParams);
            }
            catch (Exception ex)
            {
                showHelp = true;
            }

            if (showHelp || _command == null)
            {
                Program.ShowHelp(p);
#if DEBUG
                Program.ReadKey();
#endif
                return;
            }

            Console.WriteLine(_command.Execute());
#if DEBUG
            Console.WriteLine("DEBUG: Press anykey to exit");
            Program.ReadKey();
#endif

        }

        protected override void OnStop()
        {
            if (_command is IDisposable)
            {
                (_command as IDisposable).Dispose();
            }
        }
    }
}
