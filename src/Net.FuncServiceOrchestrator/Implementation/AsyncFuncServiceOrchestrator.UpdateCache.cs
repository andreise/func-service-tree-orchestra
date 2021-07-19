#nullable enable

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private Unit UpdateCache()
            =>
            ForceCacheRebuild || (cacheIsBuilt is false)
                ? RebuildCache()
                : default;

        private Unit RebuildCache()
        {
            leafsCache.Clear();
            invertedTreeCache.Clear();

            // TODO: Building cache using BFS/DFS

            leafsCache.TrimExcess();
            invertedTreeCache.TrimExcess();

            cacheIsBuilt = true;

            return default;
        }
    }
}
