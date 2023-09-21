using System;
using System.Threading;
using System.Threading.Tasks;

namespace TableMapping.Import
{
    public interface IRowSaver<in T> : IDisposable
    {
        Task SaveRowAsync(T row, CancellationToken cancellationToken);
        Task SaveRemainderAsync(CancellationToken cancellationToken);
    }
}
