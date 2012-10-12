using System;
using System.Diagnostics;
using System.ServiceModel;

namespace DistributedDeployment
{
    [ServiceContract]
    public interface IRemoteCommandService
    {
        [OperationContract]
        string Execute(string securityToken, string cmdPath);
    }
    
    internal class RemoteCommandService : IRemoteCommandService
    {
        private readonly string _securityToken;

        public RemoteCommandService(string securityToken)
        {
            _securityToken = securityToken;
        }

        public string Execute(string token, string cmdPath)
        {
            if (_securityToken != token)
            {
                return "Invalid security token";
            }

            try
            {
                var startInfo = new ProcessStartInfo("cmd")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(startInfo))
                {
                    process.StandardInput.AutoFlush = true;
                    process.StandardInput.WriteLine(cmdPath);
                    process.StandardInput.Close();

                    var result = process.StandardOutput.ReadToEnd();
                    result += process.StandardError.ReadToEnd();

                    process.WaitForExit();
                    process.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}