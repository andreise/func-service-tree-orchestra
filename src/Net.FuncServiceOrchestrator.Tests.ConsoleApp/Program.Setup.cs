#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class Program
    {
        private static IAsyncFuncService<int> leafA = null!;
        private static IAsyncFuncService<int> leafB = null!;
        private static IAsyncFuncService<int> leafC = null!;
        private static IAsyncFuncService<int> leafD = null!;
        private static IAsyncFuncService<int> leafE = null!;
        private static IAsyncFuncService<int> leafX = null!;
        private static IAsyncFuncService<int> leafY = null!;

        private static IReadOnlyList<IAsyncFuncService<int>> leafs = null!;

        private static Queue<string> notificationQueue = null!;

        private static IAsyncFuncServiceOrchestrator<int> orchestra = null!;

        private static async ValueTask SetupAsync(CancellationToken cancellationToken = default)
        {
            leafA = AsyncFuncService.CreateLinear<int>("A");
            leafB = AsyncFuncService.CreateLinear<int>("B");
            leafC = AsyncFuncService.CreateLinear<int>("C");
            leafD = AsyncFuncService.CreateLinear<int>("D");
            leafE = AsyncFuncService.CreateLinear<int>("E");
            leafX = AsyncFuncService.CreateLinear<int>("X");
            leafY = AsyncFuncService.CreateLinear<int>("Y");

            leafs = new[] { leafA, leafB, leafC, leafD, leafE, leafX, leafY };

            notificationQueue = new();

            orchestra = await BuildOrchestraAsync(cancellationToken);
        }
    }
}
