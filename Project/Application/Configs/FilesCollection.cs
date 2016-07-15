using System.Configuration;

namespace Configs
{
    public class FilesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileElement)element).FolderType;
        }

        public FileElement this[int idx]
        {
            get { return (FileElement)BaseGet(idx); }
        }
    }
}
