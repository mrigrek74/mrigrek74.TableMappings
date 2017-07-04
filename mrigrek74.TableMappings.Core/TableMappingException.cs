using System;

namespace mrigrek74.TableMappings.Core
{
    public class TableMappingException : Exception
    {
        public int Row { get; }

        public TableMappingException(int row)
        {
            Row = row;
        }

        public TableMappingException(string message, int row)
        : base(message)
        {
            Row = row;
        }

        public TableMappingException(string message, int row, Exception inner)
        : base(message, inner)
        {
            Row = row;
        }
    }
}
