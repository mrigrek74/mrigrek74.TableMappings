using System;
using System.Collections.Generic;
using mrigrek74.TableMappings.Core.Extensions;

namespace mrigrek74.TableMappings.Core
{
    public class ColumnNamesRowMapper <T>: RowMapperBase<T>
    {
        private readonly Dictionary<string, string> _propNamesColNames = new Dictionary<string, string>();

        public ColumnNamesRowMapper()
        {
            foreach (var property in Properties)
            {
                if (!property.CanWrite)
                    continue;

                var columnName = property.GetColumnName();

                if (_propNamesColNames.ContainsValue(columnName))
                    throw new InvalidOperationException($"{Strings.ColumnNameMustBeUnique}: {columnName}");

                _propNamesColNames[property.Name] = columnName;
            }
        }

        protected override void FillObject(T target, string[] row, string[] header,
            int? rowNumber, bool supressErrors)
        {
            var rowResult = new Dictionary<string, string>();
            for (int i = 0; i < row.Length; i++)
            {
                var field = row[i];
                rowResult[header[i]] = field;
            }

            foreach (var property in Properties)
            {
                if (!_propNamesColNames.ContainsKey(property.Name))
                    continue;

                var column = _propNamesColNames[property.Name];
                if (!rowResult.ContainsKey(column))
                    continue;

                string data = rowResult[_propNamesColNames[property.Name]];
                if (string.IsNullOrEmpty(data))
                    continue;

                FillProperties(target, property, data, rowNumber, supressErrors);
            }
        }
    }
}