#nullable enable

using System.Collections.Generic;

namespace System
{
    partial struct Result<TSuccess, TFailure>
    {
        public static bool operator ==(Result<TSuccess, TFailure> left, Result<TSuccess, TFailure> right)
            =>
            left.Equals(right);

        public static bool operator !=(Result<TSuccess, TFailure> left, Result<TSuccess, TFailure> right)
            =>
            left.Equals(right) is false;

        public bool Equals(Result<TSuccess, TFailure> other)
            =>
            (isSuccess == other.isSuccess) &&
            (isSuccess
                ? SuccessComparer.Equals(success, other.success)
                : FailureComparer.Equals(failure, other.failure));

        public override bool Equals(object? obj)
            =>
            obj is Result<TSuccess, TFailure> other && Equals(other);

        public override int GetHashCode()
        {
            HashCode builder = new();

            builder.Add(typeof(Result<TSuccess, TFailure>));

            builder.Add(isSuccess);

            if (isSuccess)
            {
                builder.Add(success, SuccessComparer);
            }
            else
            {
                builder.Add(failure, FailureComparer);
            }

            return builder.ToHashCode();
        }

        private static IEqualityComparer<TSuccess> SuccessComparer => EqualityComparer<TSuccess>.Default;

        private static IEqualityComparer<TFailure> FailureComparer => EqualityComparer<TFailure>.Default;
    }
}
