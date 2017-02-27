using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public abstract class TableImporterBase<T> : ITableImporter, IDisposable
    {
        protected bool EnableValidation;
        protected bool SuppressConvertTypeErrors;
        protected int? RowsLimit;
        private int? _eventInterval;

        public event EventHandler<DocumentImportEventArgs> Progress;
        protected virtual void OnProgress(DocumentImportEventArgs e)
        {
            if (_eventInterval.HasValue && e.Rows % _eventInterval.Value == 0)
            {
                Progress?.Invoke(this, e);
            }
        }

        protected RowMapper<T> RowMapper = new RowMapper<T>();
        protected IRowSaver<T> RowSaver;

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

        protected TableImporterBase(IRowSaver<T> rowSaver)
        {
            EnableValidation = false;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            RowSaver = rowSaver;

            TypeDescriptorAddProviderTransparent();
        }

        protected TableImporterBase(IRowSaver<T> rowSaver, int? eventInterval)
        {
            EnableValidation = false;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            RowSaver = rowSaver;
            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }

        protected TableImporterBase(IRowSaver<T> rowSaver, int? eventInterval, bool enableValidation)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            RowSaver = rowSaver;
            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }


        protected TableImporterBase(IRowSaver<T> rowSaver, int? eventInterval, bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = suppressConvertTypeErrors;
            RowsLimit = rowsLimit;
            RowSaver = rowSaver;
            _eventInterval = eventInterval;

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

                throw new ValidationException($"Row: {row + 1}; {aggregatedErrors}");
            }
        }

        protected void ThrowIfRowsLimitEnabled(int row)
        {
            if (RowsLimit.HasValue && row > RowsLimit)
            {
                throw new InvalidOperationException("Import is limited to " + RowsLimit + " records");
            }
        }


        public abstract void Import(string path);

        public abstract void Import(Stream stream);

        public abstract Task ImportAsync(string path, CancellationToken cancellationToken);

        public abstract Task ImportAsync(Stream stream, CancellationToken cancellationToken);

        public void Dispose()
        {
            RowSaver.Dispose();
        }
    }
}