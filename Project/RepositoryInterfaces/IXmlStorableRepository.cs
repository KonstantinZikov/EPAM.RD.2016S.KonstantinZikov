using System.IO;

namespace RepositoryInterfaces
{
    public interface StorableRepository
    {
        void Save(Stream writeStream);
        void Restore(Stream readStream);
    }
}
