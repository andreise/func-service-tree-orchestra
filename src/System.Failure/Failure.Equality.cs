#nullable enable

using System.Collections.Generic;

namespace System
{
    partial struct Failure<TFailureCode>
    {
        public static bool operator ==(Failure<TFailureCode> left, Failure<TFailureCode> right)
            =>
            left.Equals(right);

        public static bool operator !=(Failure<TFailureCode> left, Failure<TFailureCode> right)
            =>
            left.Equals(right) is false;

        public bool Equals(Failure<TFailureCode> other)
            =>
            FailureCodeComparer.Equals(FailureCode, other.FailureCode) &&
            FailureMessageComparer.Equals(FailureMessage, other.FailureMessage);

        public override bool Equals(object? obj)
            =>
            obj is Failure<TFailureCode> other && Equals(other);

        public override int GetHashCode()
            =>
            HashCode.Combine(
                typeof(Failure<TFailureCode>),
                FailureCodeComparer.GetHashCode(FailureCode),
                FailureMessageComparer.GetHashCode(FailureMessage));

        private static IEqualityComparer<TFailureCode> FailureCodeComparer => EqualityComparer<TFailureCode>.Default;

        private static StringComparer FailureMessageComparer => StringComparer.Ordinal;
    }
}
