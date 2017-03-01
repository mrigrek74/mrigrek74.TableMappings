using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace mrigrek74.TableMappings.Core
{
    public static class EnumerableExtensions
    {
        private static bool EventTypeFilter(PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p, typeof(AssociationAttribute)) as AssociationAttribute;

            if (attribute == null)
                return true;
            if (!attribute.IsForeignKey)
                return true;

            return false;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {
            Type t = typeof(T);
            var table = new DataTable();

            var properties = t.GetProperties().Where(EventTypeFilter).ToArray();
            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                table.Columns.Add(new DataColumn(property.Name, propertyType));
            }

            foreach (var entity in entities)
            {
                table.Rows
                    .Add(properties.Select(property => property.GetValue(entity, null) ?? DBNull.Value)
                        .ToArray());
            }

            return table;
        }
    }
}
