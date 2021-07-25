#nullable enable

using NUnit.Framework;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class OrchestratorTests
    {
        [Test]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 3, 31)]
        public async ValueTask TestMultipleInvokeAsync(
            int a,
            int b,
            int c,
            int d,
            int e,
            int x,
            int y,
            int a1,
            int b1,
            int c1,
            int d1,
            int e1,
            int x1,
            int y1,
            int expectedResult,
            int expectedResult1)
        {
            {
                await leafA.SetLinearSourceAsync(a, cancellationToken: default);
                await leafB.SetLinearSourceAsync(b, cancellationToken: default);
                await leafC.SetLinearSourceAsync(c, cancellationToken: default);
                await leafD.SetLinearSourceAsync(d, cancellationToken: default);
                await leafE.SetLinearSourceAsync(e, cancellationToken: default);
                await leafX.SetLinearSourceAsync(x, cancellationToken: default);
                await leafY.SetLinearSourceAsync(y, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(expectedResult, actualResult.GetSuccessOrThrow());
            }

            {
                await leafA.SetLinearSourceAsync(a1, cancellationToken: default);
                await leafB.SetLinearSourceAsync(b1, cancellationToken: default);
                await leafC.SetLinearSourceAsync(c1, cancellationToken: default);
                await leafD.SetLinearSourceAsync(d1, cancellationToken: default);
                await leafE.SetLinearSourceAsync(e1, cancellationToken: default);
                await leafX.SetLinearSourceAsync(x1, cancellationToken: default);
                await leafY.SetLinearSourceAsync(y1, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                Assert.IsTrue(actualResult.IsSuccess);
                Assert.AreEqual(expectedResult1, actualResult.GetSuccessOrThrow());
            }
        }
    }
}
