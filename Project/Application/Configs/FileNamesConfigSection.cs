using System.Configuration;

namespace Configs
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
