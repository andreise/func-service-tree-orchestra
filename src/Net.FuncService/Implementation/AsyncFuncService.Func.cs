#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        ValueTask<TValue> IAsyncFunc<TValue>.InvokeAsync(CancellationToken cancellationToken)
            =>
            IsLinear
                ? InvokeLinearAsync(cancellationToken)
                : InvokeGeneralAsync(cancellationToken);
    }
}
