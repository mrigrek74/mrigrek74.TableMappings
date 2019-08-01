using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mrigrek74.TableMappingsCore.Core.TableImport;
using OfficeOpenXml;

namespace mrigrek74.TableMappingsCore.Core.Epplus.TableImport
{
    public class XlsxTableImporter<T> : TableImporterBase<T>
    {
        private readonly string _sheetName;

        public XlsxTableImporter(
            MappingOptions mappingOptions,
            IRowSaver<T> rowSaver,
            string sheetName = null) : base(mappingOptions, rowSaver)
        {
            _sheetName = sheetName;
        }

        private void ProcessImport(ExcelWorksheet sheet, CancellationToken? ct = null)
        {
            if (sheet.Dimension == null)
                throw new TableMappingException(string.Format(Strings.XlsxPageEmpty, _sheetName), 0);

            var header = new string[sheet.Dimension.End.Column];
            if (MappingOptions.HasHeader)
            {
                for (var column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    header[column - 1] = sheet.Cells[1, column]
                                             ?.Text
                                             ?.Replace("\r", string.Empty)
                                             .Replace("\n", string.Empty)
                                             .Trim()
                                             .ToLower() ?? string.Empty;
                }
            }

            int headerCorrect = (MappingOptions.HasHeader ? 1 : 0);
            int startRow = 1 + headerCorrect;
            int row = 0;
            int indexRow = row + headerCorrect;

            for (var rowI = startRow; rowI <= sheet.Dimension.End.Row; rowI++, row++, indexRow++)
            {
                ThrowIfRowsLimitEnabled(indexRow);

                var rowResult = new string[sheet.Dimension.End.Column];

                for (var column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    var value = sheet.Cells[rowI, column]?.Value?.ToString();

                    if (MappingOptions.Trim)
                        value = value?.Trim();

                    rowResult[column - 1] = value;
                }

                var entity = RowMapper.Map(rowResult, header, indexRow, MappingOptions.SuppressConvertTypeErrors);
                ValidateRow(entity, indexRow);
                RowSaver.SaveRow(entity);
            }

            RowSaver.SaveRemainder();
        }

        public override void Import(string path)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var sheet = pck.GetExcelWorksheet(_sheetName);
                ProcessImport(sheet);
            }
        }

        public override void Import(Stream stream)
        {
            using (var pck = new ExcelPackage())
            {
                pck.Load(stream);
                var sheet = pck.GetExcelWorksheet(_sheetName);
                ProcessImport(sheet);
            }
        }

        public override async Task ImportAsync(string path, CancellationToken ct)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }

                    var sheet = pck.GetExcelWorksheet(_sheetName);
                    ProcessImport(sheet);
                }
            }, ct);
        }

        public override async Task ImportAsync(Stream stream, CancellationToken ct)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var pck = new ExcelPackage())
                {
                    pck.Load(stream);
                    var sheet = pck.GetExcelWorksheet(_sheetName);
                    ProcessImport(sheet);
                }
            }, ct);
        }
    }
}
