#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    internal sealed class InapplicableLinearAggregate<TValueList, TValue> : IAsyncFunc<TValueList, TValue>
        where TValueList : IReadOnlyList<TValue>
    {
        ValueTask<TValue> IAsyncFunc<TValueList, TValue>.InvokeAsync(TValueList arg, CancellationToken cancellationToken)
            =>
            throw new InvalidOperationException("An aggregation strategy is inapplicable for the linear function.");

        public static InapplicableLinearAggregate<TValueList, TValue> DefaultInstance => InapplicableLinearAggregateDefault.Instance;

        private static class InapplicableLinearAggregateDefault
        {
            public static readonly InapplicableLinearAggregate<TValueList, TValue> Instance = new();
        }
    }
}
