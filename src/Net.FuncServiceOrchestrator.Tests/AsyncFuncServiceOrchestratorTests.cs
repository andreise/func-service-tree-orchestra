#nullable enable

using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class AsyncFuncServiceOrchestratorTests
    {
        private IAsyncFuncService<int> serviceA = null!;
        private IAsyncFuncService<int> serviceB = null!;
        private IAsyncFuncService<int> serviceC = null!;
        private IAsyncFuncService<int> serviceD = null!;
        private IAsyncFuncService<int> serviceE = null!;
        private IAsyncFuncService<int> serviceX = null!;
        private IAsyncFuncService<int> serviceY = null!;

        private IAsyncFuncServiceOrchestrator<int> orchestra = null!;

        [SetUp]
        public async ValueTask SetupAsync()
        {
            serviceA = AsyncFuncService.CreateLinear<int>("A");
            serviceB = AsyncFuncService.CreateLinear<int>("B");
            serviceC = AsyncFuncService.CreateLinear<int>("C");
            serviceD = AsyncFuncService.CreateLinear<int>("D");
            serviceE = AsyncFuncService.CreateLinear<int>("E");
            serviceX = AsyncFuncService.CreateLinear<int>("X");
            serviceY = AsyncFuncService.CreateLinear<int>("Y");

            orchestra = await CreateOrchestratorDefaultAsync(cancellationToken: default);
        }

        private async ValueTask<IAsyncFuncServiceOrchestrator<int>> CreateOrchestratorDefaultAsync(
            CancellationToken cancellationToken)
        {
            var serviceFx = AsyncFuncService.Create(
                "Fx", new[] { serviceX }, new FuncFx());

            var serviceFy = AsyncFuncService.Create(
                "Fy", new[] { serviceY }, new FuncFy());

            var serviceFab = AsyncFuncService.Create(
                "Fab", new[] { serviceA, serviceB }, new FuncFab());

            var serviceFcd = AsyncFuncService.Create(
                "Fcd", new[] { serviceC, serviceD, serviceFab, serviceFy }, new FuncFcd());

            var serviceFe = AsyncFuncService.Create(
                "Fe", new[] { serviceE, serviceFcd, serviceFx }, new FuncFe());

            var serviceFRoot = AsyncFuncService.Create("FRoot", new[] { serviceFe }, new FuncFr());

            return await AsyncFuncServiceOrchestrator.CreateAsync(serviceFRoot, cancellationToken);
        }
    }
}
