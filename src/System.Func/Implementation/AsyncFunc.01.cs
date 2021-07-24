#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    internal sealed class AsyncFunc<T, TResult> : IAsyncFunc<T, TResult>
    {
        private readonly Func<T, CancellationToken, ValueTask<TResult>> funcAsync;

        private AsyncFunc(Func<T, CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            this.funcAsync = funcAsync;

        public static AsyncFunc<T, TResult> Create(Func<T, CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            new(funcAsync ?? throw new ArgumentNullException(nameof(funcAsync)));

        public ValueTask<TResult> InvokeAsync(T arg, CancellationToken cancellationToken = default)
            =>
            funcAsync.Invoke(arg, cancellationToken);
    }
}
