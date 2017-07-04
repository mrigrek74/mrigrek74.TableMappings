using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace mrigrek74.TableMappings.Core
{
    public class RowMapper<T>
    {
        private readonly Dictionary<string, string> _propNamesColNames = new Dictionary<string, string>();
        private readonly PropertyInfo[] _properties;

        public RowMapper()
        {
            Type targetType = typeof(T);
            _properties = targetType.GetProperties();

            foreach (var property in _properties)
            {
                if (!property.CanWrite)
                    continue;

                var columnName = ColumnNameAttribute.GetColumnName(targetType, property);
                if (columnName == null)
                    continue;

                _propNamesColNames[property.Name] = columnName;
            }
        }

        public T MapByColNames(Dictionary<string, string> row, int? rowNumber = null, bool suppressErrors = true)
        {
            var o = (T)Activator.CreateInstance(typeof(T));
            LoadByColNames(o, row, rowNumber, suppressErrors);
            return o;
        }

        public IEnumerable<T> MapByColNames(IEnumerable<Dictionary<string, string>> rows, int? rowNumber = null,
            bool suppressErrors = true)
        {
            var result = rows.Select(x =>
            {
                var o = (T)Activator.CreateInstance(typeof(T));
                LoadByColNames(o, x, rowNumber, suppressErrors);
                return o;
            });
            return result;
        }

        private void LoadByColNames(T target, Dictionary<string, string> fields, int? rowNumber, bool supressErrors)
        {
            foreach (var property in _properties)
            {
                if (!_propNamesColNames.ContainsKey(property.Name))
                    continue;

                var column = _propNamesColNames[property.Name];
                if (!fields.ContainsKey(column))
                    continue;

                string data = fields[_propNamesColNames[property.Name]];
                if (string.IsNullOrEmpty(data))
                    continue;

                try
                {
                    var o = TypeDescriptor.GetConverter(property.PropertyType)
                        .ConvertFrom(data);
                    property.SetValue(target, o);
                }
                catch (Exception ex)
                {
                    if (!supressErrors)
                        throw new TableMappingException($"{Strings.Row} {rowNumber}: {ex.Message}", rowNumber ?? 0, ex);
                }
            }
        }
    }
}