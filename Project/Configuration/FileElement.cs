using System.Configuration;

namespace Configuration
{
    public class FileElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FolderType
        {
            get { return (string)(base["type"]); }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Path
        {
            get { return (string)(base["path"]); }
            set { base["path"] = value; }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return (string)(base["name"]); }
            set { base["name"] = value; }
        }
    }
}
