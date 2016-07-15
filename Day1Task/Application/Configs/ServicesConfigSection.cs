using System.Configuration;

namespace Configs
{
    class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ReplicationElement Replication
        {
            get { return ((ReplicationElement)(base["Replication"])); }
        }
    }
}
