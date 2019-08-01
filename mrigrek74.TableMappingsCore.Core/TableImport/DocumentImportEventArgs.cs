using System;

namespace mrigrek74.TableMappingsCore.Core.TableImport
{
    public class DocumentImportEventArgs : EventArgs
    {
        public int Rows { get; }

        public DocumentImportEventArgs(int rows)
        {
            Rows = rows;
        }
    }
}
