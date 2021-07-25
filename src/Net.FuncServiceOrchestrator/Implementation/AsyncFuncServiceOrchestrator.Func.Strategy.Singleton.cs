#nullable enable

using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private async ValueTask<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>> InvokeSingletonAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            if (await root.GetLinearIsInitializedAsync(cancellationToken).ConfigureAwait(false) is false)
            {
                var linearSourceName = await root.GetNameAsync(cancellationToken).ConfigureAwait(false);
                return Failure.Create(
                    FuncServiceOrchestratorFailureCode.UninitializedLinearSource,
                    Invariant($"The linear source '{linearSourceName}' is uninitialized."));
            }

            return await root.InvokeAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
