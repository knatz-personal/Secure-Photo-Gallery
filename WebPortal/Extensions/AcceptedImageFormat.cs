using System.Configuration;

namespace WebPortal.Extensions
{
    public class AcceptedImageFormat
        : ConfigurationSection
    {
        [System.Configuration.ConfigurationProperty("ImageFormats")]
        [ConfigurationCollection(typeof(ImageFormats), AddItemName = "ImageFormat")]
        public ImageFormats ImageFormats
        {
            get
            {
                object o = this["ImageFormats"];
                return o as ImageFormats;
            }
        }

        public static ImageFormats GetConfig()
        {
            var section = (AcceptedImageFormat)ConfigurationManager.GetSection(ConfigPath);

            return section.ImageFormats ?? new ImageFormats();
        }

        private const string ConfigPath = "applicationSettings/AcceptedImageFormat";
    }
}