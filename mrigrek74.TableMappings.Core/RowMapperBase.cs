using System;
using System.Collections.Generic;
using System.Reflection;

namespace mrigrek74.TableMappings.Core
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
    }
}
