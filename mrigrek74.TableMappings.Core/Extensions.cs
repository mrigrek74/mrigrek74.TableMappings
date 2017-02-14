using System.Linq;

namespace mrigrek74.TableMappings.Core
{
   public static class Extensions
    {
        public static string ToCsvFormat(this string source, char delimiter = ';')
        {
            if (source == null)
                return string.Empty;

            source = source.Replace("\r", "").Replace("\n", "").Replace("\"", "\"\"");

            bool needSuppressExcelFormula = source.StartsWith("-")
                                             || source.StartsWith("+")
                                             || source.StartsWith("=");


            if (source.Contains(delimiter))
            {
                source = "\"" + source + "\"";
                if (needSuppressExcelFormula)
                {
                    source = "=" + source;
                }
            }
            else if (needSuppressExcelFormula)
            {
                source = "=\"" + source + "\"";
            }


            return source;
        }
    }
}
