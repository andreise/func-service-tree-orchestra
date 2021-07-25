#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class OrchestratorTests
    {
        [Test]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 3)]
        [TestCase(1, 2, 3, 4, 5, 6, 7, 31)]
        public async ValueTask TestIdempotenceAsync(
            int a,
            int b,
            int c,
            int d,
            int e,
            int x,
            int y,
            int expectedResult)
        {
            await leafA.SetLinearSourceAsync(a, cancellationToken: default);
            await leafB.SetLinearSourceAsync(b, cancellationToken: default);
            await leafC.SetLinearSourceAsync(c, cancellationToken: default);
            await leafD.SetLinearSourceAsync(d, cancellationToken: default);
            await leafE.SetLinearSourceAsync(e, cancellationToken: default);
            await leafX.SetLinearSourceAsync(x, cancellationToken: default);
            await leafY.SetLinearSourceAsync(y, cancellationToken: default);

            for (int i = 0; i < 2; i++)
            {
                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(expectedResult, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fx", "Fy", "Fab", "Fcd", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));
            }
        }
    }
}
