#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        private ValueTask<TValue> InvokeLinearAsync(CancellationToken cancellationToken)
        {
            #region Check if the task is canceled

            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<TValue>(cancellationToken);
            }

            #endregion

            if (resultCache.IsValid is false)
            {
                resultCache.Value = linearSource;
                resultCache.IsValid = true;
            }

            return ValueTask.FromResult(resultCache.Value);
        }
    }
}
