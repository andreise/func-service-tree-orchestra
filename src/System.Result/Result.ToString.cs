#nullable enable

using static System.FormattableString;

namespace System
{
    partial struct Result<TSuccess, TFailure>
    {
        public override string ToString()
            =>
            isSuccess
                ? Invariant($"A success value: {success}.")
                : Invariant($"A failure value: {failure}.");
    }
}
