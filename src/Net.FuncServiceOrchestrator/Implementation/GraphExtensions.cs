#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    internal static class GraphExtensions
    {
        public static ValueTask<bool> IsRootedTreeAsync<TValue>(
            this IAsyncFuncService<TValue> vertex,
            bool allowSingleton,
            CancellationToken cancellationToken = default)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<bool>(cancellationToken);
            }

            #endregion

            // TODO: Check if the graph is a rooted tree

            return (vertex, allowSingleton) switch
            {
                _ => ValueTask.FromResult(true)
            };
        }

        public static ValueTask<bool> IsLeafAsync<TValue>(
            this IAsyncFuncService<TValue> vertex,
            CancellationToken cancellationToken = default)
            =>
            vertex.GetIsLinearAsync(cancellationToken);
    }
}
