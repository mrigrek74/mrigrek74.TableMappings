using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using mrigrek74.TableMappings.Core.TableMapping;
using OfficeOpenXml;

namespace mrigrek74.TableMappings.Core.Epplus.TableMapping
{
    public class XlsxMapper<T> : TableMapperBase<T>
    {
        private readonly string _sheetName;

        public XlsxMapper(MappingMode mappingMode) : base(mappingMode)
        {
        }

        public XlsxMapper(MappingMode mappingMode, bool enableValidation)
            : base(mappingMode, enableValidation)
        {
        }

        public XlsxMapper(MappingMode mappingMode, string sheetName): base(mappingMode)
        {
            _sheetName = sheetName;
        }

        public XlsxMapper(MappingMode mappingMode, string sheetName, bool enableValidation)
         : base(mappingMode, enableValidation)
        {
            _sheetName = sheetName;
        }

        public XlsxMapper(MappingMode mappingMode, bool enableValidation,
            bool suppressConvertTypeErrors, int? rowsLimit)
            : base(mappingMode, enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
        }

        public XlsxMapper(MappingMode mappingMode, bool enableValidation,
            bool suppressConvertTypeErrors, int? rowsLimit, string sheetName)
            : base(mappingMode, enableValidation, suppressConvertTypeErrors, rowsLimit)
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

            var header = new string[sheet.Dimension.End.Column];
            for (var column = 1; column <= sheet.Dimension.End.Column; column++)
            {
                header[column - 1] = sheet.Cells[1, column].Text?.ToLower() ?? string.Empty;
            }

            for (var row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                ThrowIfRowsLimitEnabled(row - 1);

                var rowResult = new string[sheet.Dimension.End.Column]; 

                for (var column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    rowResult[column - 1] = sheet.Cells[row, column]?.Value?.ToString();
                }

                var entity = RowMapper.Map(rowResult, header, row, SuppressConvertTypeErrors);
                ValidateRow(entity, row - 1);
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
