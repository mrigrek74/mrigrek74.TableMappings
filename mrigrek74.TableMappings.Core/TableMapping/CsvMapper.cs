using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace mrigrek74.TableMappings.Core.TableMapping
{
    public class CsvMapper<T> : TableMapperBase<T>
    {
        private readonly Encoding _encoding;
        private readonly string[] _delimiters;

        public CsvMapper()
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvMapper(bool enableValidation)
            : base(enableValidation)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
            : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit,
            string[] delimiters) : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
        }

        public CsvMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit,
            string[] delimiters,Encoding encoding) : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = encoding;
            _delimiters = delimiters;
        }

        private IList<T> ProcessMap(TextFieldParser parser)
        {
            int row = 0;
            string[] header = { };

            var result = new List<T>();
            while (!parser.EndOfData)
            {
                ThrowIfRowsLimitEnabled(row);

                if (row == 0)
                {
                    header = parser.ReadFields();
                    if (header == null)
                        throw new TableMappingException(Strings.HeaderRowIsEmpty, row);
                }
                else
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                        continue;
                    var rowResult = new StringDictionary();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        var field = fields[i];
                        rowResult[header[i]] = field;
                    }

                    var entity = RowMapper.MapByColNames(rowResult, row + 1, SuppressConvertTypeErrors);

                    ValidateRow(entity, row);

                    result.Add(entity);
                }
                row++;
            }
            return result;
        }


        public override IList<T> Map(string path)
        {
            using (var parser = new TextFieldParser(path, _encoding)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = _delimiters
            })
            {
                return ProcessMap(parser);
            }
        }

        public override IList<T> Map(Stream stream)
        {
            using (var parser = new TextFieldParser(stream, _encoding)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = _delimiters
            })
            {
                return ProcessMap(parser);
            }
        }
    }
}
