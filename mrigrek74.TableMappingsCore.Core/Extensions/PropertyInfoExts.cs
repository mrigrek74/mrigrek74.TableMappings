using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace mrigrek74.TableMappingsCore.Core.Extensions
{
    public static class PropertyInfoExts
    {
        public static string GetColumnName(this PropertyInfo src)
            => src.GetAttr<ColumnNameAttribute>()?.ColumnName ?? src.Name;

        public static string GetColumnNameLowered(this PropertyInfo src)
            => src.GetAttr<ColumnNameAttribute>()?.ColumnNameLowered ?? src.Name.ToLower();

        public static int GetColumnNumber(this PropertyInfo src, int defaultValue)
            => src.GetAttr<ColumnNumberAttribute>()?.ColumnNumber ?? defaultValue;

        public static TA GetAttr<TA>(this PropertyInfo src) where TA : Attribute
        {
            var typeofT = typeof(TA);
            object[] attributes = src.GetCustomAttributes(typeofT, false);
            if (attributes.Length > 0)
            {
                return (TA)attributes[0];
            }


            //var metadataType = src.DeclaringType
            //    ?.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
            //    .OfType<MetadataTypeAttribute>()
            //    .FirstOrDefault();

            //if (metadataType != null)
            //{
            //    var metadata = ModelMetadataProviders.Current
            //        .GetMetadataForType(null, metadataType.MetadataClassType);

            //    var metadataProp = metadata.ModelType.GetProperty(src.Name);
            //    if (metadataProp != null)
            //    {
            //        var metadataPropAttributes = metadataProp
            //            .GetCustomAttributes(typeofT, false);

            //        if (metadataPropAttributes.Length > 0)
            //        {
            //            return (TA)metadataPropAttributes[0];
            //        }
            //    }
            //}
            return null;
        }
    }
}