using System.Configuration;

namespace Configuration
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
