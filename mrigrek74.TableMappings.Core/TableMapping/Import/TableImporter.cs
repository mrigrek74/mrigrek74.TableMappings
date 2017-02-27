using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core.TableMapping.Import
{
    public abstract class TableImporter<T> : TableMapperBase<T>, ITableImporter
    {
        protected IRowSaver<T> RowSaver;

        public event EventHandler<DocumentImportEventArgs> Progress;

        protected abstract void OnProgress(DocumentImportEventArgs e);

        protected TableImporter(IRowSaver<T> rowSaver)
        {
            RowSaver = rowSaver;
        }

        protected TableImporter(IRowSaver<T> rowSaver, bool enableValidation)
            : base(enableValidation)
        {
            RowSaver = rowSaver;
        }

        protected TableImporter(IRowSaver<T> rowSaver, bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
            : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            RowSaver = rowSaver;
        }

        public abstract void Import(string path);

        public abstract void Import(Stream stream);

        public abstract Task ImportAsync(string path, CancellationToken cancellationToken);

        public abstract Task ImportAsync(Stream stream, CancellationToken cancellationToken);
    }
}
