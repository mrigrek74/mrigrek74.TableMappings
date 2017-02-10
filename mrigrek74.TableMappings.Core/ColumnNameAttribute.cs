using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;

namespace mrigrek74.TableMappings.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string ColumnName { get; private set; }


        public ColumnNameAttribute(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("columnName is requerid", nameof(columnName));

            ColumnName = columnName.Trim().ToLower();
        }

        public static string GetColumnName(Type targetType, PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(typeof(ColumnNameAttribute), false);
            if (attributes.Length > 0)
            {
                return ((ColumnNameAttribute)attributes[0]).ColumnName;
            }

            var metadataType = targetType.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .OfType<MetadataTypeAttribute>().FirstOrDefault();

            if (metadataType != null)
            {
                var metadata = ModelMetadataProviders.Current.GetMetadataForType(null,
                    metadataType.MetadataClassType);

                var metadataProp = metadata.ModelType.GetProperty(property.Name);
                if (metadataProp != null)
                {
                    var metadataPropAttributes = metadataProp.GetCustomAttributes(typeof(ColumnNameAttribute), false);

                    if (metadataPropAttributes.Length > 0)
                    {
                        return ((ColumnNameAttribute)metadataPropAttributes[0]).ColumnName;
                    }
                }
            }
            return property.Name;
        }
    }
}
