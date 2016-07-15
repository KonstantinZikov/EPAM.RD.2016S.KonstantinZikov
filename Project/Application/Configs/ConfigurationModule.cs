using Configs;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Repositories;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;
using System;
using System.Configuration;
using Utils;

namespace Application
{
    public class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            ConfigureRepositories();
            ConfigureServices();
            Bind<ILogger>().To<DefaultLogger>().InSingletonScope();
        }
       
        private void ConfigureRepositories()
        {
            Bind<IIdGenerator>().To<FibonacciEnumerator>();
            Bind<IUserValidator>().To<UserValidator>()
                .WithConstructorArgument("maxBirthDate", new DateTime(2017,1,1));
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserRepositoryFactory>().ToFactory();
        }

        private void ConfigureServices()
        {         
            Bind<IUserServiceDistributer>().To<SignalUserServiceDistributor>();
            ConfigureReplicator();
        }

        private void ConfigureReplicator()
        {
            ServicesConfigSection section = 
                ConfigurationManager.GetSection("Services") as ServicesConfigSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException
                    ("Can't find \"Services\" config section.");
            }
            if (section.Replication == null)
            {
                throw new ConfigurationErrorsException
                    ("Can't find \"Replication\" element in \"Services\" config section.");
            }
            int slaveCount = 0;
            if (int.TryParse(section.Replication.slaveCount, out slaveCount))
            {
                Bind<IUserService>().To<DeepReplicationUserServiceSystem>()
                .InSingletonScope().WithConstructorArgument("slaveCount", slaveCount);
            }
            else
            {
                Bind<IUserService>().To<DeepReplicationUserServiceSystem>().InSingletonScope();
            }
        }
    }
}
