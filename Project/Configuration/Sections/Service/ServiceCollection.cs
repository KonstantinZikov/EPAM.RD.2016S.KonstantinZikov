using System.Configuration;

namespace Configuration
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Id;
        }

        public ServiceElement this[int idx]
        {
            get { return (ServiceElement)BaseGet(idx); }
        }
    }
}
