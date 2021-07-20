#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public class AsyncFuncServiceOrchestratorTests
    {
        [Test]
        public async ValueTask TestOrchestratorAsync()
        {
            var orchestra = await CreateOrchestratorAsync();

            var result = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.Pass();
        }

        private static async ValueTask<IAsyncFuncServiceOrchestrator<int>> CreateOrchestratorAsync()
        {
            var serviceA = AsyncFuncService.CreateLinear<int>("A");
            var serviceB = AsyncFuncService.CreateLinear<int>("B");
            var serviceC = AsyncFuncService.CreateLinear<int>("C");
            var serviceD = AsyncFuncService.CreateLinear<int>("D");
            var serviceE = AsyncFuncService.CreateLinear<int>("E");
            var serviceX = AsyncFuncService.CreateLinear<int>("X");
            var serviceY = AsyncFuncService.CreateLinear<int>("Y");

            var serviceFx = AsyncFuncService.Create("Fx", new[] { serviceX }, new FuncFx());
            var serviceFy = AsyncFuncService.Create("Fy", new[] { serviceY }, new FuncFy());
            var serviceFab = AsyncFuncService.Create("Fab", new[] { serviceA, serviceB }, new FuncFab());
            var serviceFcd = AsyncFuncService.Create("Fcd", new[] { serviceC, serviceD, serviceFab, serviceFy }, new FuncFcd());
            var serviceFe = AsyncFuncService.Create("Fe", new[] { serviceE, serviceFcd, serviceFx }, new FuncFe());

            var serviceFr = AsyncFuncService.Create("Fr", new[] { serviceFe }, new FuncFr());

            return await AsyncFuncServiceOrchestrator.CreateAsync(serviceFr, cancellationToken: default);
        }
    }

    internal sealed class FuncFx : IAsyncFunc<IReadOnlyList<int>, int>
    {
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
        {
            var x = list[0];
            return ValueTask.FromResult(x + 1);
        }
    }

    internal sealed class FuncFy : IAsyncFunc<IReadOnlyList<int>, int>
    {
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
        {
            var y = list[0];
            return ValueTask.FromResult(y + 2);
        }
    }

    internal sealed class FuncFab : IAsyncFunc<IReadOnlyList<int>, int>
    {
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
        {
            var a = list[0];
            var b = list[1];
            return ValueTask.FromResult(a + b);
        }
    }

    internal sealed class FuncFcd : IAsyncFunc<IReadOnlyList<int>, int>
    {
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
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
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
        {
            var e = list[0];
            var fcd = list[1];
            var fx = list[2];
            return ValueTask.FromResult(e + fcd + fx);
        }
    }

    internal sealed class FuncFr : IAsyncFunc<IReadOnlyList<int>, int>
    {
        public ValueTask<int> InvokeAsync(IReadOnlyList<int> list, CancellationToken cancellationToken = default)
        {
            var fe = list[0];
            return ValueTask.FromResult(fe * 2);
        }
    }
}
