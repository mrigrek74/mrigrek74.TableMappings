using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace mrigrek74.TableMappings.Core.TableMapping
{
    public abstract class TableMapperBase<T> : ITableMapper<T>
    {
        protected bool EnableValidation;
        protected bool SuppressConvertTypeErrors;
        protected int? RowsLimit;

        protected RowMapper<T> RowMapper = new RowMapper<T>();
        private void TypeDescriptorAddProviderTransparent()
        {
            var metadataType = typeof(T)
                .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .OfType<MetadataTypeAttribute>()
                .FirstOrDefault();

            if (metadataType != null)
            {
                TypeDescriptor.AddProviderTransparent(
                    new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T),
                    metadataType.MetadataClassType), typeof(T));
            }
        }

        protected TableMapperBase()
        {
            EnableValidation = false;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;

            TypeDescriptorAddProviderTransparent();
        }

        protected TableMapperBase(bool enableValidation)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;

            TypeDescriptorAddProviderTransparent();
        }


        protected TableMapperBase(bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = suppressConvertTypeErrors;
            RowsLimit = rowsLimit;

            TypeDescriptorAddProviderTransparent();
        }


        protected void ValidateRow(T entity, int row)
        {
            if (!EnableValidation)
                return;

            var context = new ValidationContext(entity, null, null);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, validationResults, true))
            {
                var aggregatedErrors = validationResults
                    .Select(x => x.ErrorMessage)
                    .Aggregate((item, next) => next + "; " + item);

                var r = row + 1;
                throw new TableMappingException($"Row: {r}; {aggregatedErrors}", r);
            }
        }

        protected void ThrowIfRowsLimitEnabled(int row)
        {
            if (RowsLimit.HasValue && row > RowsLimit)
            {
                throw new TableMappingException("Import is limited to " + RowsLimit + " records", row);
            }
        }


        public abstract IList<T> Map(string path);
        public abstract IList<T> Map(Stream stream);
    }
}