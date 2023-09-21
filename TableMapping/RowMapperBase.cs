using System;
using System.ComponentModel;
using System.Reflection;

namespace TableMapping
{
    public abstract class RowMapperBase<T>
    {
        protected readonly PropertyInfo[] Properties;

        protected RowMapperBase()
        {
            Type targetType = typeof(T);
            Properties = targetType.GetProperties();
        }

        protected abstract void FillObject(T target, string[] row, string[] header,
            int? rowNumber, bool supressErrors);

        public virtual T Map(string[] row, string[] header,
            int? rowNumber = null, bool suppressErrors = true)
        {
            var o = (T)Activator.CreateInstance(typeof(T));
            FillObject(o, row, header, rowNumber, suppressErrors);
            return o;
        }

        protected virtual void FillProperties(T target, PropertyInfo property, string data,
            int? rowNumber, bool supressErrors)
        {
            try
            {
                var o = TypeDescriptor.GetConverter(property.PropertyType)
                    .ConvertFrom(data);
                property.SetValue(target, o);
            }
            catch (Exception ex)
            {
                if (!supressErrors)
                    throw new TableMappingException($"{Strings.Row} {rowNumber}: {ex.Message}", rowNumber ?? 0, ex);
            }
        }
    }
}