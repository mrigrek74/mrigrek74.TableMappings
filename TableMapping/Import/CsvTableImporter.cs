using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TableMapping.NotVisualBasic;

namespace TableMapping.Import
{
    public class CsvTableImporter<T> : TableImporterBase<T>
    {
        private readonly Encoding _encoding;
        private readonly string[] _delimiters;

        public CsvTableImporter(MappingOptions mappingOptions, IRowSaver<T> rowSaver)
            : base(mappingOptions, rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvTableImporter(MappingOptions mappingOptions, IRowSaver<T> rowSaver, string[] delimiters)
            : base(mappingOptions, rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
        }

        private async Task ProcessImportAsync(CsvTextFieldParser parser, CancellationToken ct)
        {
            int row = 0;
            int indexRow = row + (MappingOptions.HasHeader ? 1 : 0);
            string[] header = { };


            while (!parser.EndOfData)
            {
                ct.ThrowIfCancellationRequested();

                ThrowIfRowsLimitEnabled(indexRow);

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
                    if (fields == null)
                        continue;

                    var entity = RowMapper.Map(fields, header, indexRow, MappingOptions.SuppressConvertTypeErrors);

                    ValidateRow(entity, indexRow);

                    await RowSaver.SaveRowAsync(entity, ct);
                }
                row++;
                indexRow++;
            }

            await RowSaver.SaveRemainderAsync(ct);
        }

        public override async Task ImportAsync(string path, CancellationToken ct)
        {
            using var parser = new CsvTextFieldParser(path, _encoding)
            {
                Delimiters = _delimiters,
                TrimWhiteSpace = MappingOptions.Trim
            };

            await ProcessImportAsync(parser, ct);
        }

        public override async Task ImportAsync(Stream stream, CancellationToken ct)
        {
            using var parser = new CsvTextFieldParser(stream, _encoding)
            {
                Delimiters = _delimiters,
                TrimWhiteSpace = MappingOptions.Trim
            };

            await ProcessImportAsync(parser, ct);
        }
    }
}