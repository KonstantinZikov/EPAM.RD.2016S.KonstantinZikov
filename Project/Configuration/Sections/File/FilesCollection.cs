using System.Configuration;

namespace Configuration
{
    public class FilesCollection : ConfigurationElementCollection
    {      
        public FileElement this[int idx]
        {
            get { return (FileElement)BaseGet(idx); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileElement)element).Name;
        }
    }
}
