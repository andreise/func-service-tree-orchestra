#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class OrchestratorTests
    {
        [Test]
        public async ValueTask TestRecomputePartialAsync()
        {
            {
                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsFailure);
                Assert.AreEqual(
                    FuncServiceOrchestratorFailureCode.UninitializedLinearSource,
                    actualResult.GetFailureOrThrow().FailureCode);

                Assert.AreEqual(0, notificationQueue.Count);
            }

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

                notificationQueue.Clear();
            }

            {
                await leafX.SetLinearSourceAsync(1, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(4, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fx", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));

                notificationQueue.Clear();
            }

            {
                await leafA.SetLinearSourceAsync(1, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(5, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fab", "Fcd", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));

                notificationQueue.Clear();
            }

            {
                await leafC.SetLinearSourceAsync(1, cancellationToken: default);
                await leafX.SetLinearSourceAsync(2, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(7, actualResult.GetSuccessOrThrow());

                var actualRecomputedNodes = new HashSet<string>(notificationQueue);
                var expectedRecomputedNodes = new[] { "Fcd", "Fx", "Fe" };
                Assert.True(actualRecomputedNodes.SetEquals(expectedRecomputedNodes));

                notificationQueue.Clear();
            }
        }
    }
}
