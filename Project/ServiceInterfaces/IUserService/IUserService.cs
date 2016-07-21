using Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServiceInterfaces
{
    public interface IUserService
    {
        int Add(User user);
        List<User> Search(params Func<User, bool>[] criterias);
        void Delete(User user);
        void SaveToXml(Stream writeStream);
        void RestoreFromXml(Stream readStream);
        int Id { get; }
    }
}
