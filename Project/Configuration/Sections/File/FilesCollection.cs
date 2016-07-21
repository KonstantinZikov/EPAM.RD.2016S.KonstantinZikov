using System.Configuration;

namespace Configuration
{
    public class FilesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileElement)element).Name;
        }

        public FileElement this[int idx]
        {
            get { return (FileElement)BaseGet(idx); }
        }
    }
}
