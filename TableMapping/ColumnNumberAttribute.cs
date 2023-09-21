using System;

namespace TableMapping
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
