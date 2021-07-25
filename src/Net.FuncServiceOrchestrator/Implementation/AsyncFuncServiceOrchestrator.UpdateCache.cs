#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private async ValueTask<Unit> UpdateCacheAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            if (ForceCacheRebuild || (cacheIsBuilt is false))
            {
                (leafsCache, invertedTreeCache) = await root.IsLeafAsync(cancellationToken).ConfigureAwait(false)
                    ? (LeafsCacheEmpty(), InvertedTreeCacheEmpty())
                    : await BuildCacheNonSingletonAsync(root, cancellationToken).ConfigureAwait(false);

                cacheIsBuilt = true;
            }

            return default;
        }

        private static async ValueTask<(
            IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>> Leafs,
            IReadOnlyDictionary<
                Guid,
                (IAsyncFuncServiceRemoteConfiguration<TValue> Parent, int ChildIndex)> InvertedTree)>
            BuildCacheNonSingletonAsync(
                IAsyncFuncService<TValue> root,
                CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask
                    .FromCanceled<(
                        IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>>,
                        IReadOnlyDictionary<
                            Guid,
                            (IAsyncFuncServiceRemoteConfiguration<TValue>, int)>)>(
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            #endregion

            var leafs = new List<IAsyncFuncServiceRemoteConfiguration<TValue>>();

            var invertedTree = new Dictionary<
                Guid,
                (IAsyncFuncServiceRemoteConfiguration<TValue> Parent, int ChildIndex)>();

            // Build the cache using DFS (BFS also can be used)

            var stack = new Stack<IAsyncFuncServiceRemoteConfiguration<TValue>>(new[] { root });
            do
            {
                await MoveAsync(stack, leafs, invertedTree, cancellationToken).ConfigureAwait(false);
            } while (stack.Count > 0);

            leafs.TrimExcess();
            invertedTree.TrimExcess();

            return (leafs, invertedTree);

            static async ValueTask<Unit> MoveAsync(
                Stack<IAsyncFuncServiceRemoteConfiguration<TValue>> stack,
                List<IAsyncFuncServiceRemoteConfiguration<TValue>> leafs,
                Dictionary<
                    Guid,
                    (IAsyncFuncServiceRemoteConfiguration<TValue> Parent, int ChildIndex)> invertedTree,
                CancellationToken cancellationToken)
            {
                #region Check if the task is canceled

                if (cancellationToken.IsCancellationRequested)
                {
                    return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
                }

                #endregion

                var current = stack.Peek();
                var children = await current.GetSourceConfigurationsAsync(cancellationToken).ConfigureAwait(false);

                // add check current is not the root if would adapt building for singleton (root only) too
                if (children.Count == 0)
                {
                    leafs.Add(current);
                }

                bool foundNew = false;

                for (int childIndex = 0; childIndex < children.Count; childIndex++)
                {
                    var child = children[childIndex];
                    var childId = await child.GetIdAsync(cancellationToken).ConfigureAwait(false);

                    if (invertedTree.TryAdd(childId, (Parent: current, ChildIndex: childIndex)))
                    {
                        foundNew = true;
                        stack.Push(child);
                        break;
                    }
                }

                if (foundNew is false)
                {
                    _ = stack.Pop();
                }

                return default;
            }
        }
    }
}
