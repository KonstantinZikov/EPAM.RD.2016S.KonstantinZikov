using System.Configuration;

namespace Configs
{
    class ReplicationElement : ConfigurationElement
    {
        [ConfigurationProperty("slaveCount", DefaultValue = "0", IsKey = false, IsRequired = true)]
        public string Replication
        {
            get { return (string)(base["Replication"]); }
            set { base["Replication"] = value; }
        }
    }
}
