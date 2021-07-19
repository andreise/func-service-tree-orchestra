#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    // An Asynchronous Functional (SAM) interface
    public interface IAsyncFunc<TResult>
    {
        ValueTask<TResult> InvokeAsync(CancellationToken cancellationToken = default);
    }
}
