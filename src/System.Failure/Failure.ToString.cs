#nullable enable

using static System.FormattableString;

namespace System
{
    partial struct Failure<TFailureCode>
    {
        public override string ToString()
            =>
            Invariant(
                $"A failure of {typeof(Failure<TFailureCode>).Name}: {{ Code = {FailureCode}, Message = {FailureMessage} }}");
    }
}
