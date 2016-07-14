using System.IO;

namespace RepositoryInterfaces
{
    public interface IXmlStorableRepository
    {
        void SaveToXml(Stream writeStream);
        void RestoreFromXml(Stream readStream);
    }
}
