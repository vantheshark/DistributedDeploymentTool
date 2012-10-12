using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DistributedDeployment
{
    public static class ServiceProxy 
    {
        public static void Exit(string target, int timeout = 15)
        {
            var factory = new ChannelFactory<ISignalListener>(new NetNamedPipeBinding().SetTimeout(timeout), 
                                                              new EndpointAddress("net.pipe://localhost/" + target));
            var client = factory.CreateChannel();
            client.Exit();
        }


        public static string Execute(string address, string token, string commandPath, int timeout = 15)
        {
            var tcpBinding = Server.GetDefaultNetTcpBinding().SetTimeout(timeout);
            var factory = new ChannelFactory<IRemoteCommandService>(tcpBinding, 
                new EndpointAddress(string.Format("net.tcp://{0}/{1}", address, typeof(RemoteCommandService).Name)));
            var client = factory.CreateChannel();
            return client.Execute(token, commandPath);
        }


        private static Binding SetTimeout(this Binding binding, int timeout)
        {
            var validTimeout = Math.Min(int.MaxValue, timeout*1000);
            if (validTimeout < 0)
            {
                validTimeout = int.MaxValue;
            }
            binding.SendTimeout = TimeSpan.FromMilliseconds(validTimeout);
            binding.ReceiveTimeout = TimeSpan.FromMilliseconds(timeout);
            binding.OpenTimeout = TimeSpan.FromMilliseconds(timeout);
            binding.CloseTimeout = TimeSpan.FromMilliseconds(timeout);
            return binding;
        }
    }
}