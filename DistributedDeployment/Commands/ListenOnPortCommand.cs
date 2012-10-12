using System;

namespace DistributedDeployment.Commands
{
    internal class ListenOnPortCommand : ICommand
    {
        private readonly string _securityToken;
        private readonly int _port;

        public ListenOnPortCommand(string securityToken, int port = 5555)
        {
            _securityToken = securityToken;
            _port = port;
        }

        public string Execute()
        {
            using (var host = Server.Start<IRemoteCommandService>(new RemoteCommandService(_securityToken), _port))
            {
                Console.WriteLine();
                Console.WriteLine("Distributed deployment is listening on port {0}\n. Press anykey to exit", _port);
                Console.ReadKey();
                return null;
            }
        }
    }
}
