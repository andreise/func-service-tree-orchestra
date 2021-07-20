#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private ValueTask<Unit> UpdateCacheAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Unit>(cancellationToken);
            }

            #endregion

            return ForceCacheRebuild || (cacheIsBuilt is false)
                ? RebuildCacheAsync(cancellationToken)
                : default;
        }

        private async ValueTask<Unit> RebuildCacheAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            leafsCache.Clear();
            invertedTreeCache.Clear();

            // Build using DFS (BFS also can be used)

            if (await root.IsLeafAsync(cancellationToken).ConfigureAwait(false) is false)
            {
                _ = await RebuildCacheNonSingletonAsync(cancellationToken).ConfigureAwait(false);
            }

            leafsCache.TrimExcess();
            invertedTreeCache.TrimExcess();

            cacheIsBuilt = true;

            return default;
        }

        private async ValueTask<Unit> RebuildCacheNonSingletonAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            Stack<IAsyncFuncServiceRemoteConfiguration<TValue>> stack = new(new[] { root });

            do
            {
                var current = stack.Peek();
                var children = await current.GetSourceConfigurationsAsync(cancellationToken).ConfigureAwait(false);
                bool foundUnprocessed = false;

                if (children.Count == 0) // add check current is not the root if would adapt building for singleton (root only) too
                {
                    leafsCache.Add(current);
                }

                for (
                    int childIndex = 0;
                    childIndex < children.Count && (foundUnprocessed is false);
                    childIndex++)
                {
                    var child = children[childIndex];
                    var childId = await child.GetIdAsync(cancellationToken).ConfigureAwait(false);

                    if (invertedTreeCache.ContainsKey(childId) is false)
                    {
                        foundUnprocessed = true;
                        invertedTreeCache.Add(childId, (Parent: current, ChildIndex: childIndex));
                        stack.Push(child);
                    }
                }

                if (foundUnprocessed is false)
                {
                    _ = stack.Pop();
                }
            } while (stack.Count > 0);

            return default;
        }
    }
}
