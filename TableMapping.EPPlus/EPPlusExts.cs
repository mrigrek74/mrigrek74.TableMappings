using OfficeOpenXml;
using System.Linq;

namespace TableMapping.EPPlus
{
    public static class EPPlusExts
    {
        public static ExcelWorksheet GetExcelWorksheet(this ExcelPackage pck, string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                if (!pck.Workbook.Worksheets.Any())
                    throw new TableMappingException(Strings.ExcelBookHasNoAnySheets, 0);
                return pck.Workbook.Worksheets.First();
            }

            var sheet = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName);
            if (sheet == null)
                throw new TableMappingException($"{sheetName} {Strings.notFound}", 0);
            return sheet;
        }
    }
}
