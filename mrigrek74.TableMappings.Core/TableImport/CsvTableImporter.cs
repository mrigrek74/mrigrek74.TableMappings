using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace mrigrek74.TableMappings.Core.TableImport
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

        public CsvTableImporter(
            MappingOptions mappingOptions,
            IRowSaver<T> rowSaver,
            string[] delimiters)
            : base(mappingOptions, rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
        }

        private void ProcessImport(TextFieldParser parser, CancellationToken? ct = null)
        {
            int row = 0;
            int indexRow = row + (MappingOptions.HasHeader ? 1 : 0);
            string[] header = { };


            while (!parser.EndOfData)
            {
                if (ct.HasValue && ct.Value.IsCancellationRequested)
                    ct.Value.ThrowIfCancellationRequested();

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

                    RowSaver.SaveRow(entity);
                }
                row++;
                indexRow++;
            }

            RowSaver.SaveRemainder();
        }

        public override void Import(string path)
        {
            using (var parser = new TextFieldParser(path, _encoding)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = _delimiters
            })
            {
                ProcessImport(parser);
            }
        }

        public override void Import(Stream stream)
        {
            using (var parser = new TextFieldParser(stream, _encoding)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = _delimiters
            })
            {
                ProcessImport(parser);
            }
        }

        public override async Task ImportAsync(
            string path,
            CancellationToken ct)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var parser = new TextFieldParser(path, _encoding)
                {
                    TextFieldType = FieldType.Delimited,
                    Delimiters = _delimiters
                })
                {
                    ProcessImport(parser, ct);
                }
            }, ct);
        }

        public override async Task ImportAsync(
            Stream stream,
            CancellationToken ct)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var parser = new TextFieldParser(stream, _encoding)
                {
                    TextFieldType = FieldType.Delimited,
                    Delimiters = _delimiters
                })
                {
                    ProcessImport(parser, ct);
                }

            }, ct);
        }
    }
}