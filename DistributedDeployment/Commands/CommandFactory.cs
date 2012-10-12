using System.Collections.Generic;

namespace DistributedDeployment.Commands
{
    public class CommandFactory
    {
        public static ICommand Create(Dictionary<string, object> parsedParams)
        {
            if (parsedParams.ContainsKey("port"))
            {
                return new ListenOnPortCommand(parsedParams["securityToken"] as string, int.Parse((string)parsedParams["port"]));    
            }

            
            if (parsedParams.ContainsKey("targetService"))
            {
                return new SendExitSignalCommand(parsedParams["targetService"] as string,
                                                 parsedParams.ContainsKey("waitTime") ? (int)parsedParams["waitTime"] : -1,
                                                 parsedParams.ContainsKey("killIfTimeout"));
            }

            if (parsedParams.ContainsKey("remoteCommand"))
            {
                return new ExecuteRemoteCommand(parsedParams["remoteAddress"] as string,
                                                parsedParams["securityToken"] as string,
                                                parsedParams["remoteCommand"] as string,
                                                parsedParams.ContainsKey("waitTime") ? (int)parsedParams["waitTime"] : -1);
            }
            return null;
        }
    }
}