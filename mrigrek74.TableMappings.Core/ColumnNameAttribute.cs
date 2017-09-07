using System;

namespace mrigrek74.TableMappings.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string ColumnName { get; }

        public ColumnNameAttribute(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Strings.ColumnNameIsRequired, nameof(columnName));

            ColumnName = columnName
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Trim()
                .ToLower();
        }
    }
}
