using System;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public interface IRowSaver<in T> : IDisposable
    {
        event EventHandler<DocumentImportEventArgs> Progress;
        void SaveRow(T row);
        void SaveRemainder();
    }
}
