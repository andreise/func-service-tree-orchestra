#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    partial class AsyncFuncServiceOrchestrator<TValue>
    {
        public ValueTask<TValue> InvokeAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
