#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    partial class AsyncFunc
    {
        // Asynchronous Functional (SAM) interface factory
        // https://www.nuget.org/packages/PrimeFuncPack.Core.Func.Extensions/ is recommended for use in a real project
        public static IAsyncFunc<TResult> From<TResult>(Func<CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            AsyncFunc<TResult>.Create(funcAsync);

        public static IAsyncFunc<TResult> From<TResult>(Func<TResult> func)
        {
            _ = func ?? throw new ArgumentNullException(nameof(func));

            return AsyncFunc<TResult>.Create(cancellationToken => ValueTask.FromResult(func.Invoke()));
        }
    }
}
