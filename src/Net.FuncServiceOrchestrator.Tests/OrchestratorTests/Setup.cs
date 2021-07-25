#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class OrchestratorTests
    {
        private IAsyncFuncService<int> leafA = null!;
        private IAsyncFuncService<int> leafB = null!;
        private IAsyncFuncService<int> leafC = null!;
        private IAsyncFuncService<int> leafD = null!;
        private IAsyncFuncService<int> leafE = null!;
        private IAsyncFuncService<int> leafX = null!;
        private IAsyncFuncService<int> leafY = null!;

        private IReadOnlyList<IAsyncFuncService<int>> leafs = null!;

        private IAsyncFuncServiceOrchestrator<int> orchestra = null!;

        [SetUp]
        public async ValueTask SetupAsync()
        {
            leafA = AsyncFuncService.CreateLinear<int>("A");
            leafB = AsyncFuncService.CreateLinear<int>("B");
            leafC = AsyncFuncService.CreateLinear<int>("C");
            leafD = AsyncFuncService.CreateLinear<int>("D");
            leafE = AsyncFuncService.CreateLinear<int>("E");
            leafX = AsyncFuncService.CreateLinear<int>("X");
            leafY = AsyncFuncService.CreateLinear<int>("Y");

            leafs = new[] { leafA, leafB, leafC, leafD, leafE, leafX, leafY };

            orchestra = await BuildOrchestraAsync(cancellationToken: default);
        }
    }
}
