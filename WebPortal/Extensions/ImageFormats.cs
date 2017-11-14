using System.Configuration;

namespace WebPortal.Extensions
{
    public class ImageFormats
        : ConfigurationElementCollection
    {
        public ImageFormat this[int index]
        {
            get
            {
                return base.BaseGet(index) as ImageFormat;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new ImageFormat this[string responseString]
        {
            get { return (ImageFormat)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageFormat();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ImageFormat)element).Extension;
        }
    }
}