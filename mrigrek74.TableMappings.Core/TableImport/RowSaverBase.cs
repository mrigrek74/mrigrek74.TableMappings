using System;
using System.Collections.Generic;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public abstract class RowSaverBase<T> : IRowSaver<T>
    {
        protected readonly List<T> TempList = new List<T>();

        protected abstract int InsertOrUpdateChunk { get; }

        protected abstract void ProcessImport();

        private int? _eventInterval;
        private int _totalImported;

        protected RowSaverBase(int? eventInterval)
        {
            _eventInterval = eventInterval;
        }

        public event EventHandler<DocumentImportEventArgs> Progress;
        protected virtual void OnProgress(DocumentImportEventArgs e)
        {
            if (_eventInterval.HasValue && e.Rows % _eventInterval.Value == 0)
            {
                Progress?.Invoke(this, e);
            }
        }

        private void Import()
        {
            ProcessImport();
            _totalImported += TempList.Count;
            OnProgress(new DocumentImportEventArgs(_totalImported));
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
