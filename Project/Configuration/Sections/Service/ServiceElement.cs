using System.Configuration;

namespace Configuration
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "0", IsKey = true, IsRequired = true)]
        public int Id
        {
            get { return (int)(base["id"]); }
            set { base["id"] = value; }
        }

        [ConfigurationProperty("master", DefaultValue = "false", IsKey = false, IsRequired = false)]
        public bool IsMaster
        {
            get { return (bool)(base["master"]); }
            set { base["master"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)(base["type"]); }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("repository", DefaultValue = "Default", IsKey = false, IsRequired = false)]
        public string Repository
        {
            get { return (string)(base["repository"]); }
            set { base["repository"] = value; }
        }

        [ConfigurationProperty("point", DefaultValue = "0", IsKey = false, IsRequired = false)]
        public int Point
        {
            get { return (int)(base["point"]); }
            set { base["point"] = value; }
        }
    }
}
