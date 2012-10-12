using System;
using System.ServiceModel;

namespace DistributedDeployment
{
    public class Server
    {
        public static IDisposable Start<T>(T instance, string name) where T : class
        {
            var host = new ServiceHost(instance, new[] {new Uri("net.pipe://localhost")});
            host.AddServiceEndpoint(typeof (T), new NetNamedPipeBinding(), name);

            var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;
            host.Open();
            return host;
        }

        public static IDisposable Start<T>(T instance, int port) where T : class
        {
            var tcpBaseAddress = new Uri(string.Format("net.tcp://localhost:{0}/", port));
            var tcpBinding = GetDefaultNetTcpBinding();

            var host = new ServiceHost(instance, new[] { tcpBaseAddress });
            host.AddServiceEndpoint(typeof(T), tcpBinding, instance.GetType().Name);
            
            var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;
            
            host.Open();
            return host;
        }

        internal static NetTcpBinding GetDefaultNetTcpBinding()
        {
            var tcpBinding = new NetTcpBinding
            {
                ReaderQuotas =
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 5242880,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };
            tcpBinding.MaxBufferPoolSize = 524288;
            tcpBinding.MaxBufferSize = 5242880;
            tcpBinding.MaxReceivedMessageSize = 5242880;
            tcpBinding.Security.Mode = SecurityMode.None;

            return tcpBinding;
        }
    }
}