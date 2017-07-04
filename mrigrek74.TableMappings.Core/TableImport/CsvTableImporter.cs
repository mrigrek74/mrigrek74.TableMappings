using System.Collections.Generic;
using System.IO;
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

        public CsvTableImporter(IRowSaver<T> rowSaver) : base(rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvTableImporter(IRowSaver<T> rowSaver, int? eventInterval) : base(rowSaver, eventInterval)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }


        public CsvTableImporter(IRowSaver<T> rowSaver, int? eventInterval, bool enableValidation)
            : base(rowSaver, eventInterval, enableValidation)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvTableImporter(IRowSaver<T> rowSaver, int? eventInterval, bool enableValidation,
            bool suppressConvertTypeErrors,
            int? rowsLimit)
            : base(rowSaver, eventInterval, enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvTableImporter(IRowSaver<T> rowSaver, int? eventInterval, bool enableValidation,
            bool suppressConvertTypeErrors,
            int? rowsLimit,
            string[] delimiters) : base(rowSaver, eventInterval, enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
        }

        public CsvTableImporter(IRowSaver<T> rowSaver, int eventInterval, bool enableValidation,
            bool suppressConvertTypeErrors, int? rowsLimit,
            string[] delimiters, Encoding encoding) 
            : base(rowSaver, eventInterval, enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
            _encoding = encoding;
            _delimiters = delimiters;
        }


        private void ProcessImport(TextFieldParser parser, CancellationToken? cancellationToken = null)
        {
            int row = 0;
            string[] header = { };


            while (!parser.EndOfData)
            {
                if (cancellationToken.HasValue
                   && cancellationToken.Value.IsCancellationRequested)
                    cancellationToken.Value.ThrowIfCancellationRequested();

                ThrowIfRowsLimitEnabled(row);

                if (row == 0)
                {
                    header = parser.ReadFields();
                    if (header == null)
                        throw new TableMappingException(Strings.HeaderRowIsEmpty , row);
                }
                else
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                        continue;
                    var rowResult = new Dictionary<string, string>();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        var field = fields[i];
                        rowResult[header[i]] = field;
                    }

                    var entity = RowMapper.MapByColNames(rowResult, row + 1, SuppressConvertTypeErrors);

                    ValidateRow(entity, row);

                    RowSaver.SaveRow(entity);

                    OnProgress(new DocumentImportEventArgs(row));
                }
                row++;
            }

            RowSaver.SaveRemainder();

            OnProgress(new DocumentImportEventArgs(row));
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

        public override async Task ImportAsync(string path,
                       CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var parser = new TextFieldParser(path, _encoding)
                {
                    TextFieldType = FieldType.Delimited,
                    Delimiters = _delimiters
                })
                {
                    ProcessImport(parser, cancellationToken);
                }
            }, cancellationToken);
        }

        public override async Task ImportAsync(Stream stream,
                       CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var parser = new TextFieldParser(stream, _encoding)
                {
                    TextFieldType = FieldType.Delimited,
                    Delimiters = _delimiters
                })
                {
                    ProcessImport(parser, cancellationToken);
                }

            }, cancellationToken);
        }
    }
}
