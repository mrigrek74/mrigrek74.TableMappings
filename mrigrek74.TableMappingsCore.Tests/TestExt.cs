using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mrigrek74.TableMappingsCore.Core.Extensions;
using Xunit.Abstractions;

namespace mrigrek74.TableMappingsCore.Tests
{
    public static class TestExt
    {
        private const int Take = 100;

        public static void SimpleMappingTrace<T>(this ICollection<T> items, string path, ITestOutputHelper output)
        {
            output.WriteLine(path);
            output.WriteLine("");
            output.WriteLine($"First {Take} of {items.Count}:");

            var top = items.Take(Take).ToList();

            for (int i = 0; i < top.Count; i++)
            {
                var item = top[i];
                if (i == 0)
                {
                    var line = string.Empty;
                    for (var j = 0; j < item.GetType().GetProperties().Length; j++)
                    {
                        var p = item.GetType().GetProperties()[j];
                        line += $"{p.GetColumnName()} [{p.GetColumnNumber(1)}];";
                    }

                    output.WriteLine(line);

                    output.WriteLine(string.Empty);
                }

                string line2 = string.Empty;
                foreach (var p in item.GetType().GetProperties())
                {
                    line2 += $"{p.GetValue(item, null)};";
                }
                output.WriteLine(line2);
                output.WriteLine(string.Empty);
            }
        }
    }
}
