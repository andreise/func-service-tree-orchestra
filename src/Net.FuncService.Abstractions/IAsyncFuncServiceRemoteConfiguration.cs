#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    // A FaaS microservice remote configuration interface
    public interface IAsyncFuncServiceRemoteConfiguration<TValue>
    {
        ValueTask<Guid> GetIdAsync(
            CancellationToken cancellationToken = default);

        ValueTask<string> GetNameAsync(
            CancellationToken cancellationToken = default);

        ValueTask<bool> GetIsLinearAsync(
            CancellationToken cancellationToken = default);

        ValueTask<int> GetSourceCardinalityAsync(
            CancellationToken cancellationToken = default);

        ValueTask<IReadOnlyList<IAsyncFuncServiceRemoteConfiguration<TValue>>> GetSourceConfigurationsAsync(
            CancellationToken cancellationToken = default);

        //ValueTask<Unit> ResetResultCacheAsync(
        //    CancellationToken cancellationToken = default);

        ValueTask<Unit> ResetSourceCacheAsync(
            int sourceIndex,
            CancellationToken cancellationToken = default);

        ValueTask<Unit> SetLinearSourceAsync(
            TValue value,
            CancellationToken cancellationToken = default);
    }
}
