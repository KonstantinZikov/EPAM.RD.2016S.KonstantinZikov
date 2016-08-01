using System.IO;

namespace RepositoryInterfaces
{
    public interface IStorableRepository
    {
        void Save(Stream writeStream);

        void Restore(Stream readStream);
    }
}
