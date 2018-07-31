using System;
using System.Collections.Generic;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public abstract class RowSaverBase<T> : IRowSaver<T>
    {
        protected readonly List<T> TempList = new List<T>();

        protected abstract int InsertOrUpdateChunk { get; }

        protected abstract void ProcessImport();

        protected int TotalImported;

        public event EventHandler<DocumentImportEventArgs> Progress;
        protected virtual void OnProgress(DocumentImportEventArgs e)
        {
            Progress?.Invoke(this, e);
        }

        private void Import()
        {
            ProcessImport();
            TotalImported += TempList.Count;

            OnProgress(new DocumentImportEventArgs(TotalImported));

            TempList.Clear();
        }

        public void SaveRow(T row)
        {
            TempList.Add(row);
            if (TempList.Count < InsertOrUpdateChunk)
                return;

            Import();
        }

        public void SaveRemainder() => Import();

        public virtual void Dispose()
        {
        }
    }
}
