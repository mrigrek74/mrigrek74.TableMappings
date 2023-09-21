using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TableMapping.NotVisualBasic;

namespace TableMapping.Mapping
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

        private IList<T> ProcessMap(CsvTextFieldParser parser)
        {
            int row = 0;
            int indexRow = row + (MappingOptions.HasHeader ? 1 : 0);
            string[] header = { };

            var result = new List<T>();
            while (!parser.EndOfData)
            {
                ThrowIfRowsLimitEnabled(row, indexRow);

                if (row == 0)
                {
                    if (MappingOptions.HasHeader)
                    {

                        header = parser.ReadFields();
                        header = header?.Select(x => x.ToLower()).ToArray()
                                 ?? throw new TableMappingException(Strings.HeaderRowIsEmpty, indexRow);
                    }
                }
                else
                {
                    var fields = parser.ReadFields();
                    if (fields == null || fields.All(x => x == string.Empty))
                        continue;

                    var entity = RowMapper.Map(fields, header, indexRow, MappingOptions.SuppressConvertTypeErrors);

                    ValidateRow(entity, indexRow);

                    result.Add(entity);
                }
                row++;
                indexRow++;
            }
            return result;
        }


        public override IList<T> Map(string path)
        {
            using (var parser = new CsvTextFieldParser(path, _encoding)
            {
                Delimiters = _delimiters,
                TrimWhiteSpace = MappingOptions.Trim
            })
            {
                return ProcessMap(parser);
            }
        }

        public override IList<T> Map(Stream stream)
        {
            using (var parser = new CsvTextFieldParser(stream, _encoding)
            {
                Delimiters = _delimiters,
                TrimWhiteSpace = MappingOptions.Trim
            })
            {
                return ProcessMap(parser);
            }
        }
    }
}