#nullable enable

using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using static System.FormattableString;

namespace System.Net.FuncServiceOrchestrator.Tests
{
    static partial class Program
    {
        static async Task Main()
        {
            await SetupAsync(cancellationToken: default);

            WriteLine("Fully uninitialized:");
            {
                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsFailure: {actualResult.IsFailure}"));
                WriteLine(Invariant($"  FailureCode: {actualResult.GetFailureOrThrow().FailureCode}"));
                WriteLine(Invariant($"  Computed count: {notificationQueue.Count}"));
            }
            WriteLine();

            WriteLine("Partially uninitialized:");
            {
                await leafA.SetLinearSourceAsync(0, cancellationToken: default);
                await leafB.SetLinearSourceAsync(0, cancellationToken: default);
                await leafC.SetLinearSourceAsync(0, cancellationToken: default);
                await leafD.SetLinearSourceAsync(0, cancellationToken: default);
                await leafE.SetLinearSourceAsync(0, cancellationToken: default);
                await leafX.SetLinearSourceAsync(0, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsFailure: {actualResult.IsFailure}"));
                WriteLine(Invariant($"  FailureCode: {actualResult.GetFailureOrThrow().FailureCode}"));
                WriteLine(Invariant($"  Computed count: {notificationQueue.Count}"));
            }
            WriteLine();

            WriteLine("Fully initialized:");
            {
                await leafA.SetLinearSourceAsync(0, cancellationToken: default);
                await leafB.SetLinearSourceAsync(0, cancellationToken: default);
                await leafC.SetLinearSourceAsync(0, cancellationToken: default);
                await leafD.SetLinearSourceAsync(0, cancellationToken: default);
                await leafE.SetLinearSourceAsync(0, cancellationToken: default);
                await leafX.SetLinearSourceAsync(0, cancellationToken: default);
                await leafY.SetLinearSourceAsync(0, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsSuccess: {actualResult.IsSuccess}"));
                WriteLine(Invariant($"  Result: {actualResult.GetSuccessOrThrow()}"));
                WriteLine(Invariant($"  Computed: {string.Join("; ", notificationQueue.Select(item => Invariant($"{item}")))}"));

                notificationQueue.Clear();
            }
            WriteLine();

            WriteLine("Partially updated (X):");
            {
                await leafX.SetLinearSourceAsync(1, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsSuccess: {actualResult.IsSuccess}"));
                WriteLine(Invariant($"  Result: {actualResult.GetSuccessOrThrow()}"));
                WriteLine(Invariant($"  Recomputed: {string.Join("; ", notificationQueue.Select(item => Invariant($"{item}")))}"));

                notificationQueue.Clear();
            }
            WriteLine();

            WriteLine("Partially updated (A):");
            {
                await leafA.SetLinearSourceAsync(1, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsSuccess: {actualResult.IsSuccess}"));
                WriteLine(Invariant($"  Result: {actualResult.GetSuccessOrThrow()}"));
                WriteLine(Invariant($"  Recomputed: {string.Join("; ", notificationQueue.Select(item => Invariant($"{item}")))}"));

                notificationQueue.Clear();
            }
            WriteLine();

            WriteLine("Partially updated (C, X):");
            {
                await leafC.SetLinearSourceAsync(1, cancellationToken: default);
                await leafX.SetLinearSourceAsync(2, cancellationToken: default);

                var actualResult = await orchestra.InvokeAsync(cancellationToken: default);

                WriteLine(Invariant($"  IsSuccess: {actualResult.IsSuccess}"));
                WriteLine(Invariant($"  Result: {actualResult.GetSuccessOrThrow()}"));
                WriteLine(Invariant($"  Recomputed: {string.Join("; ", notificationQueue.Select(item => Invariant($"{item}")))}"));

                notificationQueue.Clear();
            }
            WriteLine();
        }
    }
}
