#nullable enable

namespace System
{
    // A Unit type implementation
    // https://www.nuget.org/packages/PrimeFuncPack.Core.Unit/ is recommended for use in a real project
    public readonly partial struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Value;
    }
}
