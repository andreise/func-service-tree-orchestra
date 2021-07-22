#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private ValueTask<TValue> InvokeSingletonAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<TValue>(cancellationToken);
            }

            #endregion

            return root.InvokeAsync(cancellationToken);
        }
    }
}
