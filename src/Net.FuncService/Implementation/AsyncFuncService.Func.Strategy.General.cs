#nullable enable

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        private async ValueTask<TValue> InvokeGeneralAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<TValue>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            if (resultCache.IsValid is false)
            {
                await UpdateSourceCacheGeneralAsync(cancellationToken).ConfigureAwait(false);
                await UpdateResultCacheGeneralAsync(cancellationToken).ConfigureAwait(false);
                notificationQueue.Enqueue(name);
            }

            return resultCache.Value;
        }

        private async ValueTask<Unit> UpdateSourceCacheGeneralAsync(CancellationToken cancellationToken)
        {
            foreach (int i in Enumerable.Range(0, sourceCardinality))
            {
                #region Check if the task is canceled

                if (cancellationToken.IsCancellationRequested)
                {
                    return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
                }

                #endregion

                if (sourceCache[i].IsValid is false)
                {
                    sourceCache[i].Value = await sourceSuppliers[i].InvokeAsync(cancellationToken).ConfigureAwait(false);
                    sourceCache[i].IsValid = true;
                }
            }

            return default;
        }

        private async ValueTask<Unit> UpdateResultCacheGeneralAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<Unit>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            resultCache.Value = await aggregateAsync.InvokeAsync(SelectSourcesGeneral(), cancellationToken).ConfigureAwait(false);
            resultCache.IsValid = true;

            return default;
        }

        private TValue[] SelectSourcesGeneral()
            =>
            sourceCache.Select(item => item.Value).ToArray();
    }
}
