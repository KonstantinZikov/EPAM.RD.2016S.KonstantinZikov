using System.Configuration;

namespace Configuration
{
    public class PortsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("PortsSet")]
        public PortCollection Ports
        {
            get { return ((PortCollection)(base["PortsSet"])); }
        }
    }
}
