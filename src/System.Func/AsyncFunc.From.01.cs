#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    partial class AsyncFunc
    {
        // Asynchronous Functional (SAM) interface factory
        // https://www.nuget.org/packages/PrimeFuncPack.Core.Func.Extensions/ is recommended for use in a real project
        public static IAsyncFunc<T, TResult> From<T, TResult>(Func<T, CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            AsyncFunc<T, TResult>.Create(funcAsync);

        public static IAsyncFunc<T, TResult> From<T, TResult>(Func<T, TResult> func)
        {
            _ = func ?? throw new ArgumentNullException(nameof(func));

            return AsyncFunc<T, TResult>.Create((arg, cancellationToken) => ValueTask.FromResult(func.Invoke(arg)));
        }
    }
}
