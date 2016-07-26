using RepositoryInterfaces;
using System.IO;

namespace ServicesTests
{
    class PseudoUserRepositoryXml : PseudoUserRepository, StorableRepository
    {
        public bool Saved { get; set; }
        public bool Restored { get; set; }

        public void Restore(Stream readStream)
        {
            Restored = true;
        }

        public void Save(Stream writeStream)
        {
            Saved = true;
        }
    }
}
