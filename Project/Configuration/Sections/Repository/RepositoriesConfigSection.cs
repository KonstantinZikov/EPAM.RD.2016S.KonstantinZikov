using System.Configuration;

namespace Configuration
{
    public class RepositoriesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Repositories")]
        public RepositoryCollection Repositories
        {
            get { return ((RepositoryCollection)(base["Repositories"])); }
        }
    }
}
