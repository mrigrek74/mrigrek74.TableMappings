using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace mrigrek74.TableMappings.Core.TableMapping
{
    public class CsvMapper<T> : TableMapperBase<T>
    {
        private readonly Encoding _encoding;
        private readonly string[] _delimiters;

        public CsvMapper(MappingOptions mappingOptions) : base(mappingOptions)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvMapper(MappingOptions mappingOptions, string[] delimiters) : base(mappingOptions)
        {
            _encoding = Encoding.UTF8;
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
                    if (MappingOptions.HasHeader)
                    {

                        header = parser.ReadFields();
                        header = header?.Select(x => x.ToLower()).ToArray()
                                 ?? throw new TableMappingException(Strings.HeaderRowIsEmpty, row);
                    }
                }
                else
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                        continue;

                    var entity = RowMapper.Map(fields, header, row + 1, MappingOptions.SuppressConvertTypeErrors);

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