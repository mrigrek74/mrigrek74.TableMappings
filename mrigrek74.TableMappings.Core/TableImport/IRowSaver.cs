using System;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public interface IRowSaver<in T> : IDisposable
    {
        void SaveRow(T row);
        void SaveRemainder();
    }
}
