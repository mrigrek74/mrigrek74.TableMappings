using System;

namespace mrigrek74.TableMappings.Core.TableMapping.Import
{
    public interface IRowSaver<in T> : IDisposable
    {
        void SaveRow(T row);
        void SaveRemainder();
    }
}
