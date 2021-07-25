#nullable enable

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    partial class OrchestratorTests
    {
        [Test]
        public async ValueTask TestUninitializedAllAsync()
        {
            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.IsTrue(actualResult.IsFailure);
            Assert.AreEqual(FuncServiceOrchestratorFailureCode.UninitializedLinearSource, actualResult.GetFailureOrThrow().FailureCode);
        }

        [Test]
        [TestCaseSource(nameof(TestUninitializedCaseSource))]
        public async ValueTask TestUninitializedSingleAsync(int indexToExclude)
        {
            foreach (int currentIndex in Enumerable.Range(0, leafs.Count))
            {
                if (currentIndex != indexToExclude)
                {
                    await leafs[currentIndex].SetLinearSourceAsync(0);
                }
            }

            var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

            Assert.IsTrue(actualResult.IsFailure);
            Assert.AreEqual(FuncServiceOrchestratorFailureCode.UninitializedLinearSource, actualResult.GetFailureOrThrow().FailureCode);
        }

        private static IEnumerable<int> TestUninitializedCaseSource()
            =>
            Enumerable.Range(0, count: 7);
    }
}
