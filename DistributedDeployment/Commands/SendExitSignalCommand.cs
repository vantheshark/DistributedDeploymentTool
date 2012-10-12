using System;
using System.Diagnostics;
using System.Linq;

namespace DistributedDeployment.Commands
{
    internal class SendExitSignalCommand : ICommand
    {
        private readonly string _targetService;
        private readonly int _waitSeconds;
        private readonly string _killIfTimeout;

        public SendExitSignalCommand(string targetService, int waitSeconds = -1, string killIfTimeout = null)
        {
            _targetService = targetService;
            _waitSeconds = waitSeconds;
            _killIfTimeout = killIfTimeout;
        }

        public string Execute()
        {
            SendExitSignal(_targetService, _waitSeconds > 0 ? _waitSeconds : int.MaxValue);
            
            if (_waitSeconds > 0 && !string.IsNullOrEmpty(_killIfTimeout))
            {
                var processes = Process.GetProcessesByName(_killIfTimeout);
                if (processes.Any())
                {
                    Console.WriteLine("Killing {0} process", processes.Count());
                    foreach (var p in processes)
                    {
                        p.Kill();
                        return string.Format("Process {0} has been killed.", _killIfTimeout);
                    }
                }
            }
            return string.Format("{0} exited", _targetService);
        }

        private static void SendExitSignal(string target, int timeoutSeconds)
        {
            try
            {
                ServiceProxy.Exit(target, timeoutSeconds);
                Console.WriteLine("Exit signal was sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
