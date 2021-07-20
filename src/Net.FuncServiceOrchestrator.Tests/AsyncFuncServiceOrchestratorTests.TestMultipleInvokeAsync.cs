#nullable enable

using NUnit.Framework;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    public sealed partial class AsyncFuncServiceOrchestratorTests
    {
        [Test]
        public async ValueTask TestMultipleInvokeAsync()
        {
            {
                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);
                Assert.AreEqual(3, actualResult);
            }

            {
                await serviceA.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceB.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceC.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceD.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceE.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceX.SetLinearSourceAsync(0, cancellationToken: default);
                await serviceY.SetLinearSourceAsync(0, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);
                Assert.AreEqual(3, actualResult);
            }

            {
                await serviceA.SetLinearSourceAsync(1, cancellationToken: default);
                await serviceB.SetLinearSourceAsync(2, cancellationToken: default);
                await serviceC.SetLinearSourceAsync(3, cancellationToken: default);
                await serviceD.SetLinearSourceAsync(4, cancellationToken: default);
                await serviceE.SetLinearSourceAsync(5, cancellationToken: default);
                await serviceX.SetLinearSourceAsync(6, cancellationToken: default);
                await serviceY.SetLinearSourceAsync(7, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);
                Assert.AreEqual(31, actualResult);
            }
        }
    }
}
