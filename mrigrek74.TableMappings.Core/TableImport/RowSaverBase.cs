using System;
using System.Collections.Generic;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public abstract class RowSaverBase<T> : IRowSaver<T>
    {
        protected readonly List<T> TempList = new List<T>();

        protected abstract int InsertOrUpdateChunk { get; }

        protected abstract void ProcessImport();

        private readonly int? _eventInterval;
        protected int TotalImported;

        protected RowSaverBase(int? eventInterval)
        {
            _eventInterval = eventInterval;
        }

        public event EventHandler<DocumentImportEventArgs> Progress;
        protected virtual void OnProgress(DocumentImportEventArgs e, bool force = false)
        {
            if (_eventInterval.HasValue && (e.Rows > _eventInterval.Value || force))
            {
                Progress?.Invoke(this, e);
            }
        }

        private void Import(bool remainder = false)
        {
            ProcessImport();
            TotalImported += TempList.Count;
            OnProgress(new DocumentImportEventArgs(TotalImported), remainder);
            TempList.Clear();
        }

        public void SaveRow(T row)
        {
            TempList.Add(row);
            if (TempList.Count < InsertOrUpdateChunk)
                return;

            Import();
        }

        public void SaveRemainder() => Import(true);

        public virtual void Dispose()
        {
        }
    }
}
