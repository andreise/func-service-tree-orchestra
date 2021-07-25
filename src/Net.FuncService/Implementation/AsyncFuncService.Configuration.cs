#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        ValueTask<Guid> IAsyncFuncServiceRemoteConfiguration<TValue>.GetIdAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Guid>(cancellationToken);
            }

            #endregion

            return ValueTask.FromResult(id);
        }

        ValueTask<string> IAsyncFuncServiceRemoteConfiguration<TValue>.GetNameAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<string>(cancellationToken);
            }

            #endregion

            return ValueTask.FromResult(name);
        }

        ValueTask<bool> IAsyncFuncServiceRemoteConfiguration<TValue>.GetIsLinearAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<bool>(cancellationToken);
            }

            #endregion

            return ValueTask.FromResult(IsLinear);
        }

        ValueTask<bool> IAsyncFuncServiceRemoteConfiguration<TValue>.GetLinearIsInitializedAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<bool>(cancellationToken);
            }

            #endregion

            // TODO: A real microservice should return 405 Method Not Allowed code instead of throwing the invalid operation exception

            if (IsLinear is false)
            {
                throw new InvalidOperationException("The operation is applicable for the linear function service only.");
            }

            return ValueTask.FromResult(linearSourceIsInitialized);
        }

        ValueTask<int> IAsyncFuncServiceRemoteConfiguration<TValue>.GetSourceCardinalityAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<int>(cancellationToken);
            }

            #endregion

            return ValueTask.FromResult(sourceCardinality);
        }

        ValueTask<IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>>> IAsyncFuncServiceRemoteConfiguration<TValue>.GetSourceConfigurationsAsync(
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>>>(cancellationToken);
            }

            #endregion

            return ValueTask.FromResult<IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>>>(sourceSuppliers);
        }

        ValueTask<Unit> IAsyncFuncServiceRemoteConfiguration<TValue>.ResetSourceCacheAsync(
            int sourceIndex,
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Unit>(cancellationToken);
            }

            #endregion

            // TODO: A real microservice should return 405 Method Not Allowed code instead of throwing the invalid operation exception

            if (IsLinear)
            {
                throw new InvalidOperationException("Reset source cache operation is not applicable for the linear function service.");
            }

            // TODO: A real microservice should return 400 Bad Request code instead of throwing an argument exception

            if ((sourceIndex >= 0 && sourceIndex < sourceCardinality) is false)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(sourceIndex),
                    actualValue: Invariant($"{sourceIndex}"),
                    message: Invariant($"The source index must be equal to or greater than zero and less than the source cardinality ({sourceCardinality})."));
            }

            _ = InternalResetResultCache();

            sourceCache[sourceIndex].IsValid = false;
            sourceCache[sourceIndex].Value = default!;

            return default;
        }

        ValueTask<Unit> IAsyncFuncServiceRemoteConfiguration<TValue>.SetLinearSourceAsync(
            TValue value,
            CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Unit>(cancellationToken);
            }

            #endregion

            // TODO: A real microservice should return 405 Method Not Allowed code instead of throwing the invalid operation exception

            if (IsLinear is false)
            {
                throw new InvalidOperationException("Set linear source operation is applicable for the linear function service only.");
            }

            if (EqualityComparer<TValue>.Default.Equals(linearSource, value) is false)
            {
                _ = InternalResetResultCache();

                linearSource = value;
            }

            linearSourceIsInitialized = true;

            return default;
        }

        private Unit InternalResetResultCache()
        {
            resultCache.IsValid = false;
            resultCache.Value = default!;

            return default;
        }
    }
}
