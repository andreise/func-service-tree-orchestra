#nullable enable

using System.Collections.Generic;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue> : IAsyncFuncServiceOrchestrator<TValue>
    {
        // May be implemented as a configuration
        private static bool ForceCacheRebuild => false;

        private readonly IAsyncFuncService<TValue> root;

        private readonly List<IAsyncFuncService<TValue>> leafsCache = new();

        // Inverted Rooted Tree (Key: Child Id; Value: (Parent, ChildIndex))
        private readonly Dictionary<
            Guid,
            (IAsyncFuncServiceRemoteConfiguration<TValue> Parent, int ChildIndex)> invertedTreeCache = new();

        private bool cacheIsBuilt;

        private AsyncFuncServiceOrchestrator(IAsyncFuncService<TValue> root)
            =>
            this.root = root;
    }
}
