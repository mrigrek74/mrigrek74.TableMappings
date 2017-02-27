using System;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public class DocumentImportEventArgs : EventArgs
    {
        public int Rows { get; private set; }

        public DocumentImportEventArgs(int rows)
        {
            Rows = rows;
        }
    }
}
