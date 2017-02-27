using System.Collections.Generic;
using System.IO;

namespace mrigrek74.TableMappings.Core.TableMapping
{
    public abstract class TableMapper<T> : TableMapperBase<T>, ITableMapper<T>
    {
        protected TableMapper()
        {
        }

        protected TableMapper(bool enableValidation) 
            : base(enableValidation)
        {
        }

        protected TableMapper(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
            : base(enableValidation, suppressConvertTypeErrors, rowsLimit)
        {
        }

        public abstract IList<T> Map(string path);

        public abstract IList<T> Map(Stream stream);
    }
}