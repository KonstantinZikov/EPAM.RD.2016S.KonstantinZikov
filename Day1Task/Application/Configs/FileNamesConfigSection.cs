using System.Configuration;

namespace Configs
{
    class FileNamesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("FileNames")]
        public FilesCollection FileItems
        {
            get { return ((FilesCollection)(base["FileNames"])); }
        }
    }
}
