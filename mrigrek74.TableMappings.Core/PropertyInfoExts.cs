using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;

namespace mrigrek74.TableMappings.Core
{
    public static class PropertyInfoExts
    {
        public static string GetColumnName(this PropertyInfo src)
            => src.GetAttr<ColumnNameAttribute>()?.ColumnName ?? src.Name;

        public static int GetColumnNumber(this PropertyInfo src)
            => src.GetAttr<ColumnNumberAttribute>()?.ColumnNumber ?? -1;

        public static T GetAttr<T>(this PropertyInfo src) where T : Attribute
        {
            var typeofT = typeof(T);
            object[] attributes = src.GetCustomAttributes(typeofT, false);
            if (attributes.Length > 0)
            {
                return (T)attributes[0];
            }

            var metadataType = typeofT
                .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .OfType<MetadataTypeAttribute>()
                .FirstOrDefault();

            if (metadataType != null)
            {
                var metadata = ModelMetadataProviders.Current
                    .GetMetadataForType(null, metadataType.MetadataClassType);

                var metadataProp = metadata.ModelType.GetProperty(src.Name);
                if (metadataProp != null)
                {
                    var metadataPropAttributes = metadataProp
                        .GetCustomAttributes(typeofT, false);

                    if (metadataPropAttributes.Length > 0)
                    {
                        return (T)metadataPropAttributes[0];
                    }
                }
            }
            return null;
        }
    }
}
