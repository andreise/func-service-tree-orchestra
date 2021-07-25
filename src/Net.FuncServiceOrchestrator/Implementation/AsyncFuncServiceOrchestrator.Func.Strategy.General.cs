#nullable enable

using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        private async ValueTask<Result<TValue, Failure<FuncServiceOrchestratorFailureCode>>> InvokeGeneralAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<TValue>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            foreach (var leaf in leafsCache)
            {
                if (await leaf.GetLinearIsInitializedAsync(cancellationToken).ConfigureAwait(false) is false)
                {
                    var linearSourceName = await leaf.GetNameAsync(cancellationToken).ConfigureAwait(false);
                    return Failure.Create(
                        FuncServiceOrchestratorFailureCode.UninitializedLinearSource,
                        Invariant($"The linear source '{linearSourceName}' is uninitialized."));
                }
            }

            foreach (var leaf in leafsCache)
            {
                IAsyncFuncServiceRemoteConfiguration<TValue> current = leaf;
                IAsyncFuncServiceRemoteConfiguration<TValue>? next;

                while ((next = await MoveToNextAsync(current, cancellationToken).ConfigureAwait(false)) is not null)
                {
                    current = next;
                }
            }

            return await root.InvokeAsync(cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<IAsyncFuncServiceRemoteConfiguration<TValue>?> MoveToNextAsync(
            IAsyncFuncServiceRemoteConfiguration<TValue> current,
            CancellationToken cancellationToken)
        {
            var currentId = await current.GetIdAsync(cancellationToken).ConfigureAwait(false);

            if (invertedTreeCache.TryGetValue(currentId, out var next))
            {
                _ = await next.Parent.ResetSourceCacheAsync(next.ChildIndex, cancellationToken).ConfigureAwait(false);
                return next.Parent;
            }

            return null;
        }
    }
}
