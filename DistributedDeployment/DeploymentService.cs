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
        private ICommand _command;

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
            if (args == null || args.Length == 0)
            {
                args = _args;
            }

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
                Program.Logger.Debug("DD: ");
                Program.Logger.Debug(e.Message);
                Program.Logger.Debug("Try `DD --help' for more information.");
                return;
            }

            try
            {
                _command = CommandFactory.Create(parsedParams);
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message, ex);
                showHelp = true;
            }

            if (showHelp || _command == null)
            {
                Program.Logger.Debug("Passed args: " + string.Join(", ", args));
                Program.ShowHelp(p);
                return;
            }
            Program.Logger.Debug(_command.Execute());
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
