using System;
using System.Collections.Generic;
using System.ServiceModel;
using Entities;

namespace ServiceInterfaces
{
    [ServiceContract]
    public interface IWcfUserService : IUserService
    {
        [OperationContract]
        IEnumerable<User> SearchById(int id);
        [OperationContract]
        IEnumerable<User> SearchByFirstName(string name);
        [OperationContract]
        IEnumerable<User> SearchByLastName(string name);
        [OperationContract]
        IEnumerable<User> SearchByGender(Gender gender);
        [OperationContract]
        IEnumerable<User> SearchByDateOfBirth(DateTime date);
    }
}
