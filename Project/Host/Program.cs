using System;
using System.ServiceModel;
using Configuration;
using Ninject;
using ServiceInterfaces;

namespace Host
{
    public class Program
    {
        private static IUserService service;

        private static void Main(string[] args)
        {
            Initialize();
            var serviceUri = new Uri("http://localhost:8080/");
            using (var host = new ServiceHost(service, serviceUri))
            {
                host.Open();
                Console.WriteLine("Service started. To close service press any key.");
                Console.ReadKey();
            }
        }
     
        private static void Initialize()
        {
            IKernel kernel = new StandardKernel(new ConfigurationModule());
            service = kernel.Get<IUserService>();
        }
    }
}
