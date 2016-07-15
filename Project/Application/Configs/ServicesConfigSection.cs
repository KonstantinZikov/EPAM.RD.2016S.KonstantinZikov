using System.Configuration;

namespace Configs
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Replication")]
        public ReplicationElement Replication
        {
            get { return ((ReplicationElement)(base["Replication"])); }
        }
    }
}
