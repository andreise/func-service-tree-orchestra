#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace System.Net
{
    partial class AsyncFuncService
    {
        public static IAsyncFuncService<TValue> CreateLinear<TValue>(
            [AllowNull] string name)
            =>
            AsyncFuncService<TValue>.CreateLinear(
                name: name);
    }
}
