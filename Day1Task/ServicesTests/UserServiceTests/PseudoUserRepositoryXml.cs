using RepositoryInterfaces;
using System.IO;

namespace ServicesTests
{
    class PseudoUserRepositoryXml : PseudoUserRepository, IXmlStorableRepository
    {
        public bool Saved { get; set; }
        public bool Restored { get; set; }

        public void RestoreFromXml(Stream readStream)
        {
            Restored = true;
        }

        public void SaveToXml(Stream writeStream)
        {
            Saved = true;
        }
    }
}
