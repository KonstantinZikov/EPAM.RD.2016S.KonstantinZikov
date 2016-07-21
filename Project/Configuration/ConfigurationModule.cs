using Ninject.Extensions.Factory;
using Ninject.Modules;
using Repositories;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Utils;

namespace Configuration
{
    public class ConfigurationModule : NinjectModule
    {
        private Dictionary<string, Type> _userRepositories
            = new Dictionary<string, Type>();
        private Dictionary<string, IUserService> _userServices
            = new Dictionary<string, IUserService>();


        public override void Load()
        {
            Bind<ILogger>().To<DefaultLogger>().InSingletonScope();
            ConfigureRepositories();
            ConfigureServices();
            
        }
       
        private void ConfigureRepositories()
        {
            Bind<IIdGenerator>().To<FibonacciEnumerator>();
            Bind<IUserValidator>().To<UserValidator>()
                .WithConstructorArgument("maxBirthDate", new DateTime(2017,1,1));

            RepositoriesConfigSection repoSection = ConfigurationManager
                .GetSection("RepositoriesConfig") as RepositoriesConfigSection;
            if (repoSection == null)
            {
                throw new ConfigurationErrorsException
                    ("Can't find \"Repositories\" config section.");
            }
            var assembly = Assembly.Load("Repositories");
            foreach (var element in repoSection.Repositories)
            {
                var repository = element as RepositoryElement;
                if (_userRepositories.ContainsKey(repository.Name))
                {
                    throw new ConfigurationErrorsException
                        ($"Two or more repositories with name {repository.Name}.");
                }
                var type = assembly.GetType(repository.Type);
                if (type == null)
                {
                    throw new ConfigurationErrorsException
                        ($"Can't find type {repository.Name}.");
                }
                _userRepositories.Add(repository.Name, type);
            }
        }

        private void ConfigureServices()
        {         
            Bind<IUserServiceDistributer>().To<SignalUserServiceDistributor>();
            ConfigureReplicator();
        }

        private void ConfigureReplicator()
        {
            ServicesConfigSection section = 
                ConfigurationManager.GetSection("ServicesConfig") as ServicesConfigSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException
                    ("Can't find \"Services\" config section.");
            }
            var assembly = Assembly.Load("Services");
            IUserService master = null;
            List<IUserService> slaves = new List<IUserService>();
            foreach (var element in section.Services)
            {              
                var service = element as ServiceElement;
                var type = assembly.GetType(service.Type);
                Rebind(typeof(IUserRepository)).To(_userRepositories[service.Repository]);
                Rebind(typeof(IUserService)).To(type).WithConstructorArgument("id", service.Id);

                AppDomain domain = AppDomain.CreateDomain($"service{service.Id}");
                LoadAssemblies(domain);
                var logger = Kernel.GetService(typeof(ILogger));
                var idGenerator = domain.CreateInstanceAndUnwrap("Utils", "Utils.FibonacciEnumerator");
                var validator = domain.CreateInstanceAndUnwrap("Repositories","Repositories.UserValidator",new object[0],
                    BindingFlags.CreateInstance,)

               // domain.CreateInstanceAndUnwrap("Services", type.FullName,,);
                if (service.IsMaster)
                {
                    if (master != null)
                        throw new ConfigurationErrorsException("Two ore more master-services.");
                    master = Kernel.GetService(typeof(IUserService)) as IUserService;
                }
                else
                {
                    slaves.Add(Kernel.GetService(typeof(IUserService)) as IUserService);
                }
            }
            Rebind<IUserService>().To<DeepReplicationUserServiceSystem>()
                .InSingletonScope()
                .WithConstructorArgument("master", master)
                .WithConstructorArgument("slaves", slaves);
        }

        private void LoadAssemblies(AppDomain domain)
        {
            domain.Load("Services");
            domain.Load("ServiceInterfaces");
            domain.Load("Repositories");
            domain.Load("RepositoryInterfaces");
            domain.Load("Entities");
            domain.Load("Utils");
        }
    }
}
