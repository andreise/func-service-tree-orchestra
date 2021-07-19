#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    internal sealed partial class AsyncFuncService<TValue> : IAsyncFuncService<TValue>
    {
        private readonly Guid id;

        private readonly string name;

        private readonly int sourceCardinality;

        private readonly IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers;

        private readonly IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync;

        private readonly CacheItem<TValue>[] sourceCache;

        private readonly CacheItem<TValue> resultCache;

        private TValue linearSource = default!;

        private bool IsLinear => sourceCardinality > 0 is false;

        private AsyncFuncService(
            Guid id,
            string name,
            int sourceCardinality,
            IReadOnlyList<IAsyncFuncService<TValue>> sourceSuppliers,
            IAsyncFunc<IReadOnlyList<TValue>, TValue> aggregateAsync)
        {
            this.id = id;
            this.name = name;
            this.sourceCardinality = sourceCardinality;
            this.sourceSuppliers = sourceSuppliers;
            this.aggregateAsync = aggregateAsync;

            sourceCache = new CacheItem<TValue>[this.sourceCardinality];
            resultCache = new CacheItem<TValue>();
        }
    }
}
