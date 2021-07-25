#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net
{
    partial class AsyncFuncService
    {
        public static IAsyncFuncService<TValue> Create<TValue>(
            [AllowNull] string name,
            IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers,
            IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync,
            Queue<string> notificationQueue)
            =>
            AsyncFuncService<TValue>.Create(
                name: name,
                sourceSuppliers: sourceSuppliers,
                aggregateAsync: aggregateAsync,
                notificationQueue: notificationQueue);
    }
}
