using System.Collections.Generic;
using System.IO;
using System.Linq;
using mrigrek74.TableMappings.Core.TableMapping;
using OfficeOpenXml;

namespace mrigrek74.TableMappings.Core.Epplus.TableMapping
{
    public class XlsxMapper<T> : TableMapperBase<T>
    {
        private readonly string _sheetName;

        public XlsxMapper(MappingOptions mappingOptions, string sheetName = null) : base(mappingOptions)
        {
            _sheetName = sheetName;
        }

        private ExcelWorksheet GetExcelWorksheet(ExcelPackage pck)
        {
            if (string.IsNullOrEmpty(_sheetName))
            {
                if (!pck.Workbook.Worksheets.Any())
                    throw new TableMappingException(Strings.ExcelBookHasNoAnySheets, 0);
                return pck.Workbook.Worksheets.First();
            }

            var sheet = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name == _sheetName);
            if (sheet == null)
                throw new TableMappingException($"{_sheetName} {Strings.notFound}", 0);
            return sheet;
        }

        private IList<T> ProcessMap(ExcelWorksheet sheet)
        {
            var result = new List<T>();
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
                ThrowIfRowsLimitEnabled(row, indexRow);

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
                result.Add(entity);
            }

            return result;
        }

        public override IList<T> Map(string path)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var sheet = GetExcelWorksheet(pck);
                return ProcessMap(sheet);
            }
        }

        public override IList<T> Map(Stream stream)
        {
            using (var pck = new ExcelPackage())
            {
                pck.Load(stream);
                var sheet = GetExcelWorksheet(pck);
                return ProcessMap(sheet);
            }
        }
    }
}
