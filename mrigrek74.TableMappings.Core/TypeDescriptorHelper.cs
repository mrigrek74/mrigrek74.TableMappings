using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core
{
    public static class TypeDescriptorHelper<T>
    {
        public static void AddProviderTransparent()
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
    }
}
