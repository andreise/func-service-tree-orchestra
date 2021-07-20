#nullable enable

using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class AsyncFuncServiceOrchestratorTests
    {
        [Test]
        [TestCase(3)]
        public async ValueTask TestInvokeDefaultAsync(int expectedResult)
        {
            var orchestra = await CreateOrchestratorDefaultAsync(cancellationToken: default);

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
    }
}
