#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class AsyncFuncServiceOrchestratorTests
    {
        internal sealed class FuncFx : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var x = list[0];
                return ValueTask.FromResult(x + 1);
            }
        }

        internal sealed class FuncFy : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var y = list[0];
                return ValueTask.FromResult(y + 2);
            }
        }

        internal sealed class FuncFab : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var a = list[0];
                var b = list[1];
                return ValueTask.FromResult(a + b);
            }
        }

        internal sealed class FuncFcd : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var c = list[0];
                var d = list[1];
                var fab = list[2];
                var fy = list[3];
                return ValueTask.FromResult(c + d + fab + fy);
            }
        }

        internal sealed class FuncFe : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var e = list[0];
                var fcd = list[1];
                var fx = list[2];
                return ValueTask.FromResult(e + fcd + fx);
            }
        }

        internal sealed class FuncFr : IAsyncFunc<IReadOnlyList<int>, int>
        {
            public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken)
            {
                var fe = list[0];
                return ValueTask.FromResult(fe);
            }
        }
    }
}
