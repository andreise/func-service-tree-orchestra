#nullable enable

using System.Collections.Generic;

namespace System.Net
{
    internal sealed partial class AsyncFuncService<TValue> : IAsyncFuncService<TValue>
    {
        private readonly Guid id;

        private readonly string name;

        private readonly int sourceCardinality;

        private readonly IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers;

        private readonly IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync;

        private readonly Queue<string> notificationQueue;

        private readonly CacheItem<TValue>[] sourceCache;

        private readonly CacheItem<TValue> resultCache;

        private bool linearSourceIsInitialized;

        private TValue linearSource = default!;

        private bool IsLinear => sourceCardinality > 0 is false;

        private AsyncFuncService(
            Guid id,
            string name,
            int sourceCardinality,
            IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers,
            IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync,
            Queue<string> notificationQueue)
        {
            this.id = id;
            this.name = name;
            this.sourceCardinality = sourceCardinality;
            this.sourceSuppliers = sourceSuppliers;
            this.aggregateAsync = aggregateAsync;
            this.notificationQueue = notificationQueue;

            sourceCache = new CacheItem<TValue>[this.sourceCardinality];
            for (int i = 0; i < sourceCache.Length; i++)
            {
                sourceCache[i] = new();
            }

            resultCache = new();
        }
    }
}
