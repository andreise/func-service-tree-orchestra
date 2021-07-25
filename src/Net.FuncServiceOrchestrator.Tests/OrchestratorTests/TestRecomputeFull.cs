#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class OrchestratorTests
    {
        [Test]
        public async ValueTask TestRecomputeFullAsync()
        {
            {
                await leafA.SetLinearSourceAsync(0, cancellationToken: default);
                await leafB.SetLinearSourceAsync(0, cancellationToken: default);
                await leafC.SetLinearSourceAsync(0, cancellationToken: default);
                await leafD.SetLinearSourceAsync(0, cancellationToken: default);
                await leafE.SetLinearSourceAsync(0, cancellationToken: default);
                await leafX.SetLinearSourceAsync(0, cancellationToken: default);
                await leafY.SetLinearSourceAsync(0, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(3, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fx", "Fy", "Fab", "Fcd", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));
            }

            {
                await leafA.SetLinearSourceAsync(1, cancellationToken: default);
                await leafB.SetLinearSourceAsync(2, cancellationToken: default);
                await leafC.SetLinearSourceAsync(3, cancellationToken: default);
                await leafD.SetLinearSourceAsync(4, cancellationToken: default);
                await leafE.SetLinearSourceAsync(5, cancellationToken: default);
                await leafX.SetLinearSourceAsync(6, cancellationToken: default);
                await leafY.SetLinearSourceAsync(7, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(31, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fx", "Fy", "Fab", "Fcd", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));
            }
        }
    }
}
