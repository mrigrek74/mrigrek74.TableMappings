//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;

//namespace mrigrek74.TableMappingsCore.Core
//{
//    public static class TypeDescriptorHelper<T>
//    {
//        public static void AddProviderTransparent()
//        {
//            var metadataType = typeof(T)
//                .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
//                .OfType<MetadataTypeAttribute>()
//                .FirstOrDefault();

//            if (metadataType != null)
//            {
//                TypeDescriptor.AddProviderTransparent(
//                    new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T),
//                        metadataType.MetadataClassType), typeof(T));
//            }
//        }
//    }
//}
