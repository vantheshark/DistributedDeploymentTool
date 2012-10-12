using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedDeployment.Commands
{
    internal class ListenOnPortCommand : ICommand, IDisposable
    {
        private readonly string _securityToken;
        private readonly int _port;
        private readonly AutoResetEvent _waitHandler  = new AutoResetEvent(false);

        public ListenOnPortCommand(string securityToken, int port = 5555)
        {
            _securityToken = securityToken;
            _port = port;
        }

        public string Execute()
        {
            Task.Factory.StartNew(() =>
            {
                using (var host = Server.Start<IRemoteCommandService>(new RemoteCommandService(_securityToken), _port))
                {
                    _waitHandler.WaitOne();
                }
            }, TaskCreationOptions.LongRunning);
            
            if (Environment.UserInteractive)
            {
                Console.WriteLine();
                Console.WriteLine("Distributed deployment is listening on port {0}\n", _port);
                Console.WriteLine("Press ALT-X or ALT-Q to exit");
                do
                {
                    var keyinfo = Console.ReadKey();
                    if ((keyinfo.Modifiers & ConsoleModifiers.Alt) != 0 &&
                        (keyinfo.Key == ConsoleKey.X || keyinfo.Key == ConsoleKey.Q))
                    {
                        _waitHandler.Set();
                        break;
                    }
                } while (true);
            }
            return null;
        }

        public void Dispose()
        {
            _waitHandler.Dispose();
        }
    }
}
