using System.Configuration;

namespace Configuration
{
    public class FileNamesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("FileNames")]
        public FilesCollection FileItems
        {
            get { return ((FilesCollection)(base["FileNames"])); }
        }
    }
}
