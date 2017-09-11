using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mrigrek74.TableMappings.Core.Extensions;

namespace mrigrek74.TableMappings.Tests
{
    public static class TestExt
    {
        private const int Take = 100;

        public  static void SimpleMappingTrace<T>(this ICollection<T> items, string path)
        {
            Trace.WriteLine(path);
            Trace.WriteLine("");
            Trace.WriteLine($"First {Take} of {items.Count}:");

            var top = items.Take(Take).ToList();

            for (int i = 0; i < top.Count; i++)
            {
                var item = top[i];
                if (i == 0)
                {
                    for (var j = 0; j < item.GetType().GetProperties().Length; j++)
                    {
                        var p = item.GetType().GetProperties()[j];
                        Trace.Write($"{p.GetColumnName()} [{p.GetColumnNumber(1)}];");
                    }
                    Trace.WriteLine(string.Empty);
                }
                foreach (var p in item.GetType().GetProperties())
                {
                    Trace.Write($"{p.GetValue(item, null)};");
                }
                Trace.WriteLine(string.Empty);
            }
        }
    }
}
