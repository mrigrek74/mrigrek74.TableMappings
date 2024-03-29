﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using TableMapping.Mapping;
using OfficeOpenXml;

namespace TableMapping.EPPlus.Mapping
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
                    throw new TableMappingException("В Excel-книге нет страниц", 0);
                return pck.Workbook.Worksheets.First();
            }

            var sheet = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name == _sheetName);
            if (sheet == null)
                throw new TableMappingException($"{_sheetName} не найдена", 0);
            return sheet;
        }

        private IList<T> ProcessMap(ExcelWorksheet sheet)
        {
            var result = new List<T>();

            if (sheet.Dimension == null)
                throw new TableMappingException($"xlsx-cтраница {_sheetName} пустая", 0);

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

            int rowFix = MappingOptions.HasHeader ? 1 : 0;
            for (var row = 1 + rowFix; row <= sheet.Dimension.End.Row; row++)
            {
                ThrowIfRowsLimitEnabled(row, row + rowFix);

                var rowResult = new string[sheet.Dimension.End.Column];

                for (var column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    var value = sheet.Cells[row, column]?.Value?.ToString();

                    if (MappingOptions.Trim)
                        value = value?.Trim();

                    rowResult[column - 1] = value;
                }

                var entity = RowMapper.Map(rowResult, header, row, MappingOptions.SuppressConvertTypeErrors);
                ValidateRow(entity, row);
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