#nullable enable

namespace System
{
    // A Result monad prototype
    // https://www.nuget.org/packages/PrimeFuncPack.Core.Result/ is recommended for use in a real project
    public readonly partial struct Result<TSuccess, TFailure> : IEquatable<Result<TSuccess, TFailure>>
        where TFailure : struct
    {
        private readonly bool isSuccess;

        private readonly TSuccess success;

        private readonly TFailure failure;

        public bool IsSuccess => isSuccess;

        public bool IsFailure => isSuccess is false;

        public TSuccess GetSuccessOrThrow()
            =>
            isSuccess
                ? success
                : throw new InvalidOperationException("The result instance does not represent a success.");

        public TFailure GetFailureOrThrow()
            =>
            isSuccess is false
                ? failure
                : throw new InvalidOperationException("The result instance does not represent a failure.");

        public Result(TSuccess success)
        {
            isSuccess = true;
            this.success = success;
            failure = default;
        }

        public Result(TFailure failure)
        {
            isSuccess = false;
            this.failure = failure;
            success = default!;
        }

        public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
            =>
            new(success);

        public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
            =>
            new(failure);
    }
}
