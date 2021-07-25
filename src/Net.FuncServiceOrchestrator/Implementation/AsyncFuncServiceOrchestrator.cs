#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue> : IAsyncFuncServiceOrchestrator<TValue>
    {
        // May be implemented as a configuration
        private static bool ForceCacheRebuild => false;

        private readonly IAsyncFuncService<TValue> root;

        private bool cacheIsBuilt;

        private IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>> leafsCache
            = LeafsCacheEmpty();

        // Inverted Rooted Tree (Key: Child Id; Value: (Parent, ChildIndex))
        private IReadOnlyDictionary<
            Guid, (IAsyncFuncServiceRemoteConfiguration<TValue> Parent, int ChildIndex)> invertedTreeCache
            = InvertedTreeCacheEmpty();

        private AsyncFuncServiceOrchestrator(IAsyncFuncService<TValue> root)
            =>
            this.root = root;

        private static IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>> LeafsCacheEmpty()
            =>
            Array.Empty<IAsyncFuncServiceRemoteConfiguration<TValue>>();

        private static IReadOnlyDictionary<Guid, (IAsyncFuncServiceRemoteConfiguration<TValue>, int)> InvertedTreeCacheEmpty()
            =>
            new Dictionary<Guid, (IAsyncFuncServiceRemoteConfiguration<TValue>, int)>();
    }
}
