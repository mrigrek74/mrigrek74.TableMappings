using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace mrigrek74.TableMappings.Core
{
    public class ColumnNumbersRowMapper<T> : RowMapperBase<T>
    {
        private readonly Dictionary<string, int> _propNamesColNumbers = new Dictionary<string, int>();

        public ColumnNumbersRowMapper()
        {
            for (var i = 0; i < Properties.Length; i++)
            {
                if (!Properties[i].CanWrite)
                    continue;

                var columnNumber = Properties[i].GetColumnNumber(i);

                if(_propNamesColNumbers.ContainsValue(columnNumber))
                    throw new InvalidOperationException($"{Strings.ColumnNumberMustBeUnique}: {columnNumber}");

                _propNamesColNumbers[Properties[i].Name] = columnNumber;
            }
        }

        protected override void FillObject(T target, string[] row, string[] header,
            int? rowNumber, bool supressErrors)
        {
            foreach (var property in Properties)
            {
                if (!_propNamesColNumbers.ContainsKey(property.Name))
                    continue;

                var column = _propNamesColNumbers[property.Name];
                if (column > row.Length - 1)
                    continue;

                string data = row[_propNamesColNumbers[property.Name]];
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
