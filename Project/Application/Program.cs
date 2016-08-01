using System;
using System.Threading;
using Configuration;
using Entities;
using Ninject;
using ServiceInterfaces;

namespace Application
{
    public class Program
    {
        private static IUserService service;

        private static void Main(string[] args)
        {
            Initialize();
            Console.ReadKey();
        }

        private static void AddUsers()
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

        private static void Initialize()
        {
            IKernel kernel = new StandardKernel(new ConfigurationModule());
            service = kernel.Get<IUserService>();
        }
    }
}
