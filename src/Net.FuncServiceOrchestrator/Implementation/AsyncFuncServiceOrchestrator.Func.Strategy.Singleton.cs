#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private async ValueTask<TValue> InvokeSingletonAsync(CancellationToken cancellationToken)
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
