#nullable enable

namespace System.Net
{
    public interface IAsyncFuncServiceOrchestrator<TValue> : IAsyncFunc<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>>
    {
    }
}
