using System.Configuration;

namespace Configuration
{
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "Repository")]
    public class RepositoryCollection : ConfigurationElementCollection
    {       
        public RepositoryElement this[int idx]
        {
            get { return (RepositoryElement)BaseGet(idx); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RepositoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RepositoryElement)element).Name;
        }
    }
}
