using System;

namespace DistributedDeployment.Commands
{
    internal class ListenOnPortCommand : ICommand, IDisposable
    {
        private readonly string _securityToken;
        private readonly int _port;
        private IDisposable _host;

        public ListenOnPortCommand(string securityToken, int port = 5555)
        {
            _securityToken = securityToken;
            _port = port;
        }

        public string Execute()
        {
            _host = Server.Start<IRemoteCommandService>(new RemoteCommandService(_securityToken), _port);
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
                        break;
                    }
                } while (true);
            }
            return null;
        }

        public void Dispose()
        {
            if (_host != null)
            {
                _host.Dispose();
            }
        }
    }
}
