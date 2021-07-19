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

            return await root.InvokeAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
