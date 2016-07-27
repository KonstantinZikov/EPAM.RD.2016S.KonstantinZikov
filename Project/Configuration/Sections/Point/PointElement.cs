using System.Configuration;

namespace Configuration
{
    public class PointElement : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "0", IsKey = true, IsRequired = true)]
        public int Key
        {
            get { return (int)(base["key"]); }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("ip", DefaultValue = "127.0.0.1", IsKey =  false, IsRequired = false)]
        public string Ip
        {
            get { return (string)(base["ip"]); }
            set { base["ip"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "50000", IsKey = false, IsRequired = false)]
        public int Port
        {
            get { return (int)(base["port"]); }
            set { base["port"] = value; }
        }
    }
}
