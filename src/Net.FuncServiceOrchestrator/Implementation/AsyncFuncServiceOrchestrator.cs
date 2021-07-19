#nullable enable

using System.Collections.Generic;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue> : IAsyncFuncServiceOrchestrator<TValue>
    {
        // May be implemented as a configuration
        private bool ForceCacheRebuild => false;

        private readonly IAsyncFuncService<TValue> root;

        private readonly List<IAsyncFuncService<TValue>> leafsCache = new();

        // Inverted Rooted Tree (Key: child Id; Value: parent)
        private readonly Dictionary<Guid, IAsyncFuncService<TValue>> invertedTreeCache = new();

        private bool cacheIsBuilt;

        private AsyncFuncServiceOrchestrator(IAsyncFuncService<TValue> root)
            =>
            this.root = root;
    }
}
