using System.IO;
using RepositoryInterfaces;

namespace ServicesTests
{
    public class PseudoUserRepositoryXml : PseudoUserRepository, IStorableRepository
    {
        public bool Saved { get; set; }

        public bool Restored { get; set; }

        public void Restore(Stream readStream)
        {
            this.Restored = true;
        }

        public void Save(Stream writeStream)
        {
            this.Saved = true;
        }
    }
}
