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
        [TestCase(3)]
        public async ValueTask TestOrchestratorDefaultAsync(int expectedResult)
        {
            var orchestra = await CreateOrchestratorDefaultAsync(cancellationToken: default);

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 3)]
        [TestCase(1, 2, 3, 4, 5, 6, 7, 31)]
        public async ValueTask TestOrchestratorSingleRunAsync(
            int a,
            int b,
            int c,
            int d,
            int e,
            int x,
            int y,
            int expectedResult)
        {
            var orchestra = await CreateOrchestratorAsync(a, b, c, d, e, x, y, cancellationToken: default);

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private static async ValueTask<IAsyncFuncServiceOrchestrator<int>> CreateOrchestratorDefaultAsync(
            CancellationToken cancellationToken)
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

            return await AsyncFuncServiceOrchestrator.CreateAsync(serviceFr, cancellationToken);
        }

        private static async ValueTask<IAsyncFuncServiceOrchestrator<int>> CreateOrchestratorAsync(
            int a,
            int b,
            int c,
            int d,
            int e,
            int x,
            int y,
            CancellationToken cancellationToken)
        {
            var serviceA = AsyncFuncService.CreateLinear<int>("A");
            await serviceA.SetLinearSourceAsync(a, cancellationToken);

            var serviceB = AsyncFuncService.CreateLinear<int>("B");
            await serviceB.SetLinearSourceAsync(b, cancellationToken);

            var serviceC = AsyncFuncService.CreateLinear<int>("C");
            await serviceC.SetLinearSourceAsync(c, cancellationToken);

            var serviceD = AsyncFuncService.CreateLinear<int>("D");
            await serviceD.SetLinearSourceAsync(d, cancellationToken);

            var serviceE = AsyncFuncService.CreateLinear<int>("E");
            await serviceE.SetLinearSourceAsync(e, cancellationToken);

            var serviceX = AsyncFuncService.CreateLinear<int>("X");
            await serviceX.SetLinearSourceAsync(x, cancellationToken);

            var serviceY = AsyncFuncService.CreateLinear<int>("Y");
            await serviceY.SetLinearSourceAsync(y, cancellationToken);

            var serviceFx = AsyncFuncService.Create("Fx", new[] { serviceX }, new FuncFx());
            var serviceFy = AsyncFuncService.Create("Fy", new[] { serviceY }, new FuncFy());
            var serviceFab = AsyncFuncService.Create("Fab", new[] { serviceA, serviceB }, new FuncFab());
            var serviceFcd = AsyncFuncService.Create("Fcd", new[] { serviceC, serviceD, serviceFab, serviceFy }, new FuncFcd());
            var serviceFe = AsyncFuncService.Create("Fe", new[] { serviceE, serviceFcd, serviceFx }, new FuncFe());

            var serviceFr = AsyncFuncService.Create("Fr", new[] { serviceFe }, new FuncFr());

            return await AsyncFuncServiceOrchestrator.CreateAsync(serviceFr, cancellationToken);
        }
    }

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
