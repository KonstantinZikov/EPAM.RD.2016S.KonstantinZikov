using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using Entities;

namespace ServiceInterfaces
{
    [ServiceContract]
    public interface IUserService
    {
        int Id { get; }

        [OperationContract]
        int Add(User user);

        [OperationContract]
        void Delete(User user);

        List<User> Search(params Func<User, bool>[] criterias);  
             
        void Save(Stream writeStream);

        void Restore(Stream readStream);       
    }
}
