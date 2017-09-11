namespace mrigrek74.TableMappings.Core
{
    public enum MappingMode
    {
        ByName,
        ByNumber
    }

    public class MappingOptions
    {
        public MappingMode MappingMode { get; set; } = MappingMode.ByName;
        public bool EnableValidation { get; set; }
        public bool SuppressConvertTypeErrors { get; set; } = true;
        public int? RowsLimit { get; set; }

        private bool _hasHeader;

        public bool HasHeader
        {
            get => MappingMode == MappingMode.ByName || _hasHeader;
            set => _hasHeader = value;
        }
    }
}