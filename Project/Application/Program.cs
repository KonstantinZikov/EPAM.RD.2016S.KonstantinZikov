using System;
using Ninject;
using ServiceInterfaces;
using Entities;
using Configuration;

namespace Application
{
    class Program
    {
        static IUserService service;

        static void Main(string[] args)
        {
            Initialize();
            service.Add(new User
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(2015, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            });
        }

        static void Initialize()
        {
            IKernel kernel = new StandardKernel(new ConfigurationModule());
            service = kernel.Get<IUserService>();
        }
    }
}
