#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net
{
    partial class AsyncFuncService<TValue>
    {
        public static AsyncFuncService<TValue> Create(
            [AllowNull] string name,
            IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers,
            IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync)
        {
            _ = sourceSuppliers ?? throw new ArgumentNullException(nameof(sourceSuppliers));
            _ = aggregateAsync ?? throw new ArgumentNullException(nameof(aggregateAsync));

            return new(
                id: Guid.NewGuid(),
                name: name ?? string.Empty,
                sourceCardinality: sourceSuppliers.Count,
                sourceSuppliers: sourceSuppliers,
                aggregateAsync: aggregateAsync);
        }
    }
}
