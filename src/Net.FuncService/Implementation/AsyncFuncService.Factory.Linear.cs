#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        public static AsyncFuncService<TValue> CreateLinear(
            [AllowNull] string name)
        {
            return new(
                id: Guid.NewGuid(),
                name: name ?? string.Empty,
                sourceCardinality: 0,
                sourceSuppliers: Array.Empty<IAsyncFuncService<TValue>>(),
                aggregateAsync: InapplicableLinearAggregate<IReadOnlyList<TValue>, TValue>.DefaultInstance);
        }
    }
}
