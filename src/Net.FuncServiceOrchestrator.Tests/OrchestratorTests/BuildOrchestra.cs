#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class OrchestratorTests
    {
        private async ValueTask<IAsyncFuncServiceOrchestrator<int>> BuildOrchestraAsync(
            CancellationToken cancellationToken)
        {
            var nodeFx = AsyncFuncService.Create(
                "Fx",
                BuildSuppliers(
                    leafX),
                BuildAggregate(
                    src => src[0] + 1),
                notificationQueue);

            var nodeFy = AsyncFuncService.Create(
                "Fy",
                BuildSuppliers(
                    leafY),
                BuildAggregate(
                    src => src[0] + 2),
                notificationQueue);

            var nodeFab = AsyncFuncService.Create(
                "Fab",
                BuildSuppliers(
                    leafA, leafB),
                BuildAggregate(
                    src => src[0] + src[1]),
                notificationQueue);

            var nodeFcd = AsyncFuncService.Create(
                "Fcd",
                BuildSuppliers(
                    leafC, leafD, nodeFab, nodeFy),
                BuildAggregate(
                    src => src[0] + src[1] + src[2] + src[3]),
                notificationQueue);

            var nodeFe = AsyncFuncService.Create(
                "Fe",
                BuildSuppliers(
                    leafE, nodeFcd, nodeFx),
                BuildAggregate(
                    src => src[0] + src[1] + src[2]),
                notificationQueue);

            return await AsyncFuncServiceOrchestrator.CreateAsync(nodeFe, cancellationToken);

            static IAsyncFuncService<TValue>[] BuildSuppliers<TValue>(params IAsyncFuncService<TValue>[] services)
                =>
                services;

            static IAsyncFunc<IReadOnlyList<int>, int> BuildAggregate(Func<IReadOnlyList<int>, int> func)
                =>
                AsyncFunc.From(func);
        }
    }
}
