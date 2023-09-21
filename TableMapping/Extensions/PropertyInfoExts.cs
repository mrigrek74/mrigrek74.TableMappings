using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TableMapping.Extensions
{
    public static class PropertyInfoExts
    {
        public static string GetColumnName(this PropertyInfo src)
            => src.GetAttr<ColumnNameAttribute>()?.ColumnName ?? src.Name;

        public static string GetColumnNameLowered(this PropertyInfo src)
            => src.GetAttr<ColumnNameAttribute>()?.ColumnNameLowered ?? src.Name.ToLower();

        public static int GetColumnNumber(this PropertyInfo src, int defaultValue)
            => src.GetAttr<ColumnNumberAttribute>()?.ColumnNumber ?? defaultValue;

        public static TA GetAttr<TA>(this PropertyInfo src) where TA : Attribute
        {
            var typeofT = typeof(TA);
            object[] attributes = src.GetCustomAttributes(typeofT, false);
            if (attributes.Length > 0)
            {
                return (TA)attributes[0];
            }
            return null;
        }
    }
}