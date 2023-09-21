using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TableMapping.Import
{
    public abstract class RowSaverBase<T> : IRowSaver<T>
    {
        protected readonly List<T> TempList = new List<T>();

        protected abstract int InsertOrUpdateChunk { get; }
        protected abstract Task ProcessImportAsync(CancellationToken cancellationToken);

        protected int TotalImported;

        private async Task ImportAsync(CancellationToken cancellationToken)
        {
            await ProcessImportAsync(cancellationToken);
            TotalImported += TempList.Count;

            TempList.Clear();
        }

        public async Task SaveRowAsync(T row, CancellationToken cancellationToken)
        {
            TempList.Add(row);
            if (TempList.Count < InsertOrUpdateChunk)
                return;

            await ImportAsync(cancellationToken);
        }

        public async Task SaveRemainderAsync(CancellationToken cancellationToken) => await ImportAsync(cancellationToken);

        public virtual void Dispose()
        {
        }
    }
}
