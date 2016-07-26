using System.Configuration;

namespace Configuration
{
    public class PortElement : ConfigurationElement
    {
        [ConfigurationProperty("val", DefaultValue = "50000", IsKey = true, IsRequired = true)]
        public int Val
        {
            get { return (int)(base["val"]); }
            set { base["val"] = value; }
        }             
    }
}
