#nullable enable

using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class AsyncFuncServiceOrchestratorTests
    {
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
            var orchestra = await CreateOrchestratorSingleRunAsync(a, b, c, d, e, x, y, cancellationToken: default);

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private static async ValueTask<IAsyncFuncServiceOrchestrator<int>> CreateOrchestratorSingleRunAsync(
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
}
