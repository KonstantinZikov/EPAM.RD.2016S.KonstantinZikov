using System;
using Ninject;
using ServiceInterfaces;
using Entities;
using Configuration;
using System.Threading.Tasks;
using System.Threading;

namespace Application
{
    class Program
    {
        static IUserService service;

        static void Main(string[] args)
        {
            Initialize();
            //AddUsers();
            Console.ReadKey();
        }

        static void AddUsers()
        {
            for (int i = 0; i < 100; i++)
            {
                service.Add(new User
                {
                    FirstName = $"Vasya{i}",
                    LastName = "Pupkin",
                    DateOfBirth = new DateTime(2015, 1, 1),
                    Gender = Gender.Male,
                    PersonalId = "1"
                });
                Thread.Sleep(200);
            }
            
        }

        static void Initialize()
        {
            IKernel kernel = new StandardKernel(new ConfigurationModule());
            service = kernel.Get<IUserService>();
        }
    }
}
