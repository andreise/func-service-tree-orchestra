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
            IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync,
            Queue<string> notificationQueue)
        {
            _ = sourceSuppliers ?? throw new ArgumentNullException(nameof(sourceSuppliers));
            _ = aggregateAsync ?? throw new ArgumentNullException(nameof(aggregateAsync));
            _ = notificationQueue ?? throw new ArgumentNullException(nameof(notificationQueue));

            return new(
                id: Guid.NewGuid(),
                name: name ?? string.Empty,
                sourceCardinality: sourceSuppliers.Count,
                sourceSuppliers: sourceSuppliers,
                aggregateAsync: aggregateAsync,
                notificationQueue: notificationQueue);
        }
    }
}
