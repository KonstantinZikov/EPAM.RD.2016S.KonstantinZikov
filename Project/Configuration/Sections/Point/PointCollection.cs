using System.Configuration;

namespace Configuration
{
    [ConfigurationCollection(typeof(PointElement), AddItemName = "Point")]
    public class PointCollection : ConfigurationElementCollection
    {        
        public PointElement this[int idx]
        {
            get { return (PointElement)BaseGet(idx); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PointElement)element).Key;
        }
    }
}
