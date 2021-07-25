#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    internal sealed class AsyncFunc<TResult> : IAsyncFunc<TResult>
    {
        private readonly Func<CancellationToken, ValueTask<TResult>> funcAsync;

        private AsyncFunc(Func<CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            this.funcAsync = funcAsync;

        public static AsyncFunc<TResult> Create(Func<CancellationToken, ValueTask<TResult>> funcAsync)
            =>
            new(funcAsync ?? throw new ArgumentNullException(nameof(funcAsync)));

        public ValueTask<TResult> InvokeAsync(CancellationToken cancellationToken = default)
            =>
            funcAsync.Invoke(cancellationToken);
    }
}
