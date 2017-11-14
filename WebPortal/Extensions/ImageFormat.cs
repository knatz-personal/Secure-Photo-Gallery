using System.Configuration;

namespace WebPortal.Extensions
{
    public class ImageFormat : ConfigurationElement
    {
        [ConfigurationProperty("extension", IsRequired = true)]
        public string Extension
        {
            get
            {
                return this["extension"] as string;
            }
        }

        [ConfigurationProperty("signature", IsRequired = true)]
        public string Signature
        {
            get
            {
                return this["signature"] as string;
            }
        }
    }
}