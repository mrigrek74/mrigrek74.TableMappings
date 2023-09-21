using System;

namespace TableMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string ColumnNameLowered { get; }
        public string ColumnName { get; }

        public ColumnNameAttribute(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Strings.ColumnNameIsRequired, nameof(columnName));

            ColumnName = columnName.Trim();
            ColumnNameLowered = ColumnName.ToLower();
        }
    }
}