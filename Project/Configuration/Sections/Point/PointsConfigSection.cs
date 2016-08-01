using System.Configuration;

namespace Configuration
{
    public class PointsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("SlavePoints")]
        public PointCollection SlavePoints
        {
            get { return (PointCollection)base["SlavePoints"]; }
        }

        [ConfigurationProperty("MasterPointPool")]
        public PointCollection MasterPointPool
        {
            get { return (PointCollection)base["MasterPointPool"]; }
        }
    }
}
