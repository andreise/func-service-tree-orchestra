#nullable enable

namespace System.Net
{
    internal sealed class CacheItem<TValue>
    {
        public bool IsValid { get; set; }

        public TValue Value { get; set; } = default!;
    }
}
