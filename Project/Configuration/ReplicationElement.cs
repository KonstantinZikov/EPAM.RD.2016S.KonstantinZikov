using System.Configuration;

namespace Configuration
{
    public class ReplicationElement : ConfigurationElement
    {
        [ConfigurationProperty("slaveCount", DefaultValue = "0", IsKey = false, IsRequired = true)]
        public string slaveCount
        {
            get { return (string)(base["slaveCount"]); }
            set { base["slaveCount"] = value; }
        }
    }
}
