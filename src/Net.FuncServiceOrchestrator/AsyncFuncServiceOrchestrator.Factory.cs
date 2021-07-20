#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    public static partial class AsyncFuncServiceOrchestrator
    {
        public static async ValueTask<IAsyncFuncServiceOrchestrator<TValue>> CreateAsync<TValue>(
            IAsyncFuncService<TValue> root,
            CancellationToken cancellationToken = default)
            =>
            await AsyncFuncServiceOrchestrator<TValue>.CreateAsync(root, cancellationToken).ConfigureAwait(false);
    }
}
