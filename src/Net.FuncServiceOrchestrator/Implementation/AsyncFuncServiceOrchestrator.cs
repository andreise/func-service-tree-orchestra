#nullable enable

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue> : IAsyncFuncServiceOrchestrator<TValue>
    {
        private readonly IAsyncFuncService<TValue> root;

        private AsyncFuncServiceOrchestrator(IAsyncFuncService<TValue> root)
            =>
            this.root = root;
    }
}
