using System;

namespace mrigrek74.TableMappings.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNumberAttribute : Attribute
    {
        public int ColumnNumber { get; }

        public ColumnNumberAttribute(int columnNumber)
        {
            if (columnNumber < 0)
                throw new ArgumentException(nameof(columnNumber));

            ColumnNumber = columnNumber;
        }
    }
}
