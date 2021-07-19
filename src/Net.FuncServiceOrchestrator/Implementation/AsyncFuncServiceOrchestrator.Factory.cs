#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        public static async ValueTask<AsyncFuncServiceOrchestrator<TValue>> CreateAsync(
            IAsyncFuncService<TValue> root,
            CancellationToken cancellationToken = default)
        {
            _ = root ?? throw new ArgumentNullException(nameof(root));

            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return await ValueTask.FromCanceled<AsyncFuncServiceOrchestrator<TValue>>(cancellationToken).ConfigureAwait(false);
            }

            #endregion

            if (await root.IsTreeAsync(cancellationToken).ConfigureAwait(false))
            {
                throw new ArgumentException(message: "The root cannot be a leaf.", paramName: nameof(root));
            }

            return new(root: root);
        }
    }
}
