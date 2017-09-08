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
        public bool HasHeader { get; set; }

        public MappingOptions()
        {
            if (MappingMode == MappingMode.ByName)
                HasHeader = true;
        }
    }
}
