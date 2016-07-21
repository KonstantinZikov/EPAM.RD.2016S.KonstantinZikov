using System.Configuration;

namespace Configuration
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection Services
        {
            get { return ((ServiceCollection)(base["Services"])); }
        }
    }
}
