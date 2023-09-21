using OfficeOpenXml;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TableMapping.Import;

namespace TableMapping.EPPlus.Import
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

        private async Task ProcessImportAsync(ExcelWorksheet sheet, CancellationToken ct)
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
                await RowSaver.SaveRowAsync(entity, ct);
            }

            await RowSaver.SaveRemainderAsync(ct);
        }

        public override async Task ImportAsync(string path, CancellationToken ct)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var sheet = pck.GetExcelWorksheet(_sheetName);
               await ProcessImportAsync(sheet, ct);
            }
        }

        public override async Task ImportAsync(Stream stream, CancellationToken ct)
        {
            using (var pck = new ExcelPackage())
            {
                pck.Load(stream);
                var sheet = pck.GetExcelWorksheet(_sheetName);
                await ProcessImportAsync(sheet, ct);
            }
        }        
         
    }
}