#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    // An Asynchronous Functional (SAM) interface
    // https://www.nuget.org/packages/PrimeFuncPack.Core.Func/ is recommended for use in a real project
    public interface IAsyncFunc<TResult>
    {
        ValueTask<TResult> InvokeAsync(CancellationToken cancellationToken = default);
    }
}
