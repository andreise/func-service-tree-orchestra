#nullable enable

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private async ValueTask<TValue> InvokeGeneralAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<TValue>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            foreach (var leaf in leafsCache)
            {
                IAsyncFuncService<TValue> current = leaf;
                IAsyncFuncService<TValue>? next;

                while ((next = await MoveToNextAsync(current, cancellationToken).ConfigureAwait(false)) is not null)
                {
                    current = next;
                }
            }

            return await root.InvokeAsync(cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<IAsyncFuncService<TValue>?> MoveToNextAsync(
            IAsyncFuncService<TValue> current,
            CancellationToken cancellationToken)
        {
            var currentId = await current.GetIdAsync(cancellationToken).ConfigureAwait(false);

            if (invertedTreeCache.TryGetValue(currentId, out var next))
            {
                await next.Parent.ResetSourceCacheAsync(next.ChildIndex, cancellationToken).ConfigureAwait(false);
                return next.Parent;
            }

            return null;
        }
    }
}
