using System.Configuration;

namespace Configuration
{
    [ConfigurationCollection(typeof(PortElement), AddItemName = "Port")]
    public class PortCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PortElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PortElement)element).Val;
        }

        public PortElement this[int idx]
        {
            get { return (PortElement)BaseGet(idx); }
        }
    }
}
