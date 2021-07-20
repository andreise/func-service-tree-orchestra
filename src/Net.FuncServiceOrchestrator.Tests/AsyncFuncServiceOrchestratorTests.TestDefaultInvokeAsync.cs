#nullable enable

using NUnit.Framework;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class AsyncFuncServiceOrchestratorTests
    {
        [Test]
        [TestCase(3)]
        public async ValueTask TestDefaultInvokeAsync(int expectedResult)
        {
            var orchestra = await CreateOrchestratorDefaultAsync(cancellationToken: default);

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
