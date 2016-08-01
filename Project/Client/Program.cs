using System;
using System.Threading;
using Client.UserService;

namespace Application
{
    public class Program
    {
        private static WcfUserServiceClient service;

        private static void Main(string[] args)
        {
            service = new WcfUserServiceClient("BasicHttpBinding_IWcfUserService");
            AddUsers();
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
    }
}
