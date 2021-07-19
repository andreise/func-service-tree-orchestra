#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        public async ValueTask<TValue> InvokeAsync(CancellationToken cancellationToken = default)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<TValue>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            foreach (var leaf in leafsCache)
            {
                // TODO: Implement

                //var current = leaf;
                //var currentId = await current.GetIdAsync().ConfigureAwait(false);
                
                //while (invertedTreeCache.TryGetValue(currentId, out var next))
                //{

                //}
            }

            return await root.InvokeAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
