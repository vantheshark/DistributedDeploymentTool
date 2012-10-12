using System.ServiceModel;

namespace DistributedDeployment
{
    [ServiceContract]
    public interface ISignalListener
    {
        [OperationContract]
        void Exit();
    }
}
