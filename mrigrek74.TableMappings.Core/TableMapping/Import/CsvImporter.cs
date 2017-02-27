using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace mrigrek74.TableMappings.Core.TableMapping.Import
{
    public class CsvImporter<T>: TableImporter<T>
    {
        private readonly Encoding _encoding;
        private readonly string[] _delimiters;
        
        protected override void OnProgress(DocumentImportEventArgs e)
        {
            if (_eventInterval.HasValue && e.Rows % _eventInterval.Value == 0)
            {
                Progress?.Invoke(this, e);
            }
        }

        private int? _eventInterval;

        private void TypeDescriptorAddProviderTransparent()
        {
            var metadataType = typeof(T)
                .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .OfType<MetadataTypeAttribute>()
                .FirstOrDefault();

            if (metadataType != null)
            {
                TypeDescriptor.AddProviderTransparent(
                    new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T),
                    metadataType.MetadataClassType), typeof(T));
            }
        }

        public CsvImporter(IRowSaver<T> rowSaver) : base(rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
            TypeDescriptorAddProviderTransparent();
        }

        public CsvImporter(IRowSaver<T> rowSaver, Encoding encoding, string[] delimiters) : base(rowSaver)
        {
            _encoding = encoding;
            _delimiters = delimiters;
            TypeDescriptorAddProviderTransparent();
        }

        public CsvImporter(IRowSaver<T> rowSaver, string[] delimiters) : base(rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
            TypeDescriptorAddProviderTransparent();
        }

        public CsvImporter(IRowSaver<T> rowSaver, string[] delimiters, int eventInterval) : base(rowSaver)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;

            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }

        private void ProcessReadAndSave(TextFieldParser parser, CancellationToken? cancellationToken = null)
        {
            int row = 0;
            string[] header = { };

            while (!parser.EndOfData)
            {
                //if (RowsLimit.HasValue && (row + 1) > RowsLimit)
                //{
                //    throw new InvalidOperationException("Import is limited to " + (RowsLimit - 1) + " records");
                //}

                if (cancellationToken.HasValue
                    && cancellationToken.Value.IsCancellationRequested)
                    cancellationToken.Value.ThrowIfCancellationRequested();

                if (row == 0)
                {
                    header = parser.ReadFields();
                    if (header == null)
                        throw new InvalidOperationException("Header row is empty");
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

                    T entity;
                    try
                    {
                        entity = RowMapper.MapByColNames(rowResult, false);
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException($"Row: {row + 1}; {ex.Message}");
                    }

                    var context = new ValidationContext(entity, null, null);
                    ICollection<ValidationResult> validationResults = new List<ValidationResult>();
                    if (!Validator.TryValidateObject(entity, context, validationResults, true))
                    {
                        var aggregatedErrors = validationResults
                            .Select(x => x.ErrorMessage)
                            .Aggregate((item, next) => next + "; " + item);

                        throw new ValidationException($"Row: {row + 1}; {aggregatedErrors}");
                    }

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
                ProcessReadAndSave(parser);
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
                ProcessReadAndSave(parser);
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
                    ProcessReadAndSave(parser, cancellationToken);
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
                    ProcessReadAndSave(parser, cancellationToken);
                }

            }, cancellationToken);
        }
    }
}
