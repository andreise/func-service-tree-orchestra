#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace System
{
    public static class Failure
    {
        public static Failure<TFailureCode> Create<TFailureCode>(
            TFailureCode failureCode, [AllowNull] string failureMessage)
            where TFailureCode : struct
            =>
            new(failureCode, failureMessage);
    }

    // A Failure type prototype
    // https://www.nuget.org/packages/PrimeFuncPack.Core.Failure/ is recommended for use in a real project
    public readonly partial struct Failure<TFailureCode> : IEquatable<Failure<TFailureCode>>
        where TFailureCode : struct
    {
        public TFailureCode FailureCode { get; }

        public string FailureMessage => failureMessage ?? string.Empty;

        private readonly string? failureMessage;

        public Failure(TFailureCode failureCode, [AllowNull] string failureMessage)
        {
            FailureCode = failureCode;
            this.failureMessage = string.IsNullOrEmpty(failureMessage) ? null : failureMessage;
        }

        public Failure<TResultFailureCode> MapFailureCode<TResultFailureCode>(
            Func<TFailureCode, TResultFailureCode> mapFailureCode)
            where TResultFailureCode : struct
        {
            _ = mapFailureCode ?? throw new ArgumentNullException(nameof(mapFailureCode));

            return new(mapFailureCode.Invoke(FailureCode), FailureMessage);
        }
    }
}
