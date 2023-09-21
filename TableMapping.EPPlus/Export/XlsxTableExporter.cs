using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using TableMapping.Export;
using TableMapping.Extensions;

namespace TableMapping.EPPlus.Export
{
    public class XlsxTableExporter<T> : ITableExporter<T> where T : class
    {
        private const string DefaultSheetName = "Sheet1";
        private void WriteRows(IList<T> rows, ExcelWorksheet sheet)
        {
            Type t = typeof(T);
            var props = t.GetProperties();
            for (int j = 0; j < props.Length; j++)
            {
                sheet.Cells[1, j + 1].Value =
                    props[j].GetColumnName();
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < props.Length; j++)
                {
                    var val = props[j].GetValue(rows[i], null);
                    var valStr = (val ?? string.Empty).ToString();
                    sheet.Cells[i + 2, j + 1].Value = valStr;
                }
            }
        }

        public void Export(IList<T> rows, Stream stream)
        {
            using (var pck = new ExcelPackage())
            {
                var sheet = pck.Workbook.Worksheets.Add(DefaultSheetName);
                WriteRows(rows, sheet);
                pck.SaveAs(stream);
            }
        }

        public void Export(IList<T> rows, string path)
        {
            using (var pck = new ExcelPackage())
            {
                var sheet = pck.Workbook.Worksheets.Add(DefaultSheetName);
                WriteRows(rows, sheet);
                pck.SaveAs(new FileInfo(path));
            }
        }
    }
}
