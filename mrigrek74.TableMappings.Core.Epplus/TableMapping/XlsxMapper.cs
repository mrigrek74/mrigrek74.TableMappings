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

        public XlsxMapper()
        {
        }

        public XlsxMapper(bool enableValidation)
            : base(enableValidation)
        {
        }

        public XlsxMapper(string sheetName)
        {
            _sheetName = sheetName;
        }

        public XlsxMapper(string sheetName, bool enableValidation)
         : base(enableValidation)
        {
            _sheetName = sheetName;
        }

        public XlsxMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
            : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
        }

        public XlsxMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit, string sheetName)
            : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
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
                header[column - 1] = sheet.Cells[1, column].Text;
            }

            for (var row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                ThrowIfRowsLimitEnabled(row - 1);

                var rowResult = new StringDictionary();
                for (var column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    rowResult[header[column - 1]] = sheet.Cells[row, column].Text;
                }

                var entity = RowMapper.MapByColNames(rowResult, row, SuppressConvertTypeErrors);
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
