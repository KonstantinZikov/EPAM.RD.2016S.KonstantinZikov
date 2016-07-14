using System.Configuration;

namespace Configs
{
    class FilesCollection : ConfigurationElementCollection
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
