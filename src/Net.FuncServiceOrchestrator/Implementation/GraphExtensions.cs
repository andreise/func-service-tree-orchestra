#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    internal static class GraphExtensions
    {
        public static ValueTask<bool> IsTreeAsync<TValue>(
            this IAsyncFuncService<TValue> vertex,
            CancellationToken cancellationToken = default)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<bool>(cancellationToken);
            }

            #endregion

            // TODO: Check if the graph is a tree, not a forest

            return vertex switch
            {
                _ => ValueTask.FromResult(true)
            };
        }
    }
}
