#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        async ValueTask<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>> IAsyncFunc<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>>.InvokeAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<TValue>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            _ = await UpdateCacheAsync(cancellationToken).ConfigureAwait(false);

            return await root.IsLeafAsync(cancellationToken).ConfigureAwait(false)
                ? await InvokeSingletonAsync(cancellationToken).ConfigureAwait(false)
                : await InvokeGeneralAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
