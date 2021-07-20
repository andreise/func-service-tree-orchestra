#nullable enable

using NUnit.Framework;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class AsyncFuncServiceOrchestratorTests
    {
        [Test]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 3)]
        [TestCase(1, 2, 3, 4, 5, 6, 7, 31)]
        public async ValueTask TestInvokeAsync(
            int a,
            int b,
            int c,
            int d,
            int e,
            int x,
            int y,
            int expectedResult)
        {
            await serviceA.SetLinearSourceAsync(a, cancellationToken: default);
            await serviceB.SetLinearSourceAsync(b, cancellationToken: default);
            await serviceC.SetLinearSourceAsync(c, cancellationToken: default);
            await serviceD.SetLinearSourceAsync(d, cancellationToken: default);
            await serviceE.SetLinearSourceAsync(e, cancellationToken: default);
            await serviceX.SetLinearSourceAsync(x, cancellationToken: default);
            await serviceY.SetLinearSourceAsync(y, cancellationToken: default);

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
