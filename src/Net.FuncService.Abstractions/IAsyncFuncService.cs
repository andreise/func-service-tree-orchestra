#nullable enable

namespace System.Net
{
    // A FaaS microservice interface
    public interface IAsyncFuncService<TValue> :
        IAsyncFunc<TValue>,                             // GET function
        IAsyncFuncServiceRemoteConfiguration<TValue>    // Remote config functions
    {
    }
}
