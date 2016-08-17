using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Ninject.Modules;
using Repositories;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;
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
            this.ConfigureRepositories();
            this.ConfigureServices();          
        }
       
        private void ConfigureRepositories()
        {
            Bind<IIdGenerator>().To<FibonacciEnumerator>();
            Bind<IUserValidator>().To<UserValidator>()
                .WithConstructorArgument("maxBirthDate", new DateTime(2017, 1, 1));

            RepositoriesConfigSection repoSection = ConfigurationManager
                .GetSection("RepositoriesConfig") as RepositoriesConfigSection;
            if (repoSection == null)
            {
                throw new ConfigurationErrorsException("Can't find \"Repositories\" config section.");
            }

            var assembly = Assembly.Load("Repositories");
            foreach (var element in repoSection.Repositories)
            {
                var repository = element as RepositoryElement;
                if (this._userRepositories.ContainsKey(repository.Name))
                {
                    throw new ConfigurationErrorsException($"Two or more repositories with name {repository.Name}.");
                }

                var type = assembly.GetType(repository.Type);
                if (type == null)
                {
                    throw new ConfigurationErrorsException($"Can't find type {repository.Name}.");
                }

                this._userRepositories.Add(repository.Name, type);
            }
        }

        private void ConfigureServices()
        {         
            Bind<IUserServiceDistributer>().To<DefaultUserServiceDistributor>();
            this.ConfigureReplicator();
        }

        private void ConfigureReplicator()
        {
            ServicesConfigSection section = 
                ConfigurationManager.GetSection("ServicesConfig") as ServicesConfigSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Can't find \"Services\" config section.");
            }

            var assembly = Assembly.Load("Services");
            IUserService master = null;
            List<IUserService> slaves = new List<IUserService>();
            var masterPoints = new List<IPEndPoint>();           
            var pointSection = ConfigurationManager.GetSection("PointsConfig") as PointsConfigSection;

            foreach (var p in pointSection.MasterPointPool)
            {
                var point = p as PointElement;
                masterPoints.Add(new IPEndPoint(IPAddress.Parse(point.Ip), point.Port));
            }

            var slavePoints = new Dictionary<int, IPEndPoint>();
            foreach (var p in pointSection.SlavePoints)
            {
                var point = p as PointElement;
                slavePoints.Add(point.Key, new IPEndPoint(IPAddress.Parse(point.Ip), point.Port));
            }

            foreach (var element in section.Services)
            {              
                var service = element as ServiceElement;
                var type = assembly.GetType(service.Type);
                IPEndPoint point = null;
                if (service.Point != 0)
                {
                    point = slavePoints[service.Point];
                }
                                            
                Rebind(typeof(IUserRepository)).To(this._userRepositories[service.Repository]);
                AppDomain domain = AppDomain.CreateDomain($"service{service.Id}");
                this.LoadAssemblies(domain);
                var logger = Kernel.GetService(typeof(ILogger));
                var idGenerator = domain.CreateInstanceAndUnwrap("Utils", "Utils.FibonacciEnumerator");
                var validator = domain.CreateInstanceAndUnwrap(
                    "Repositories",
                    "Repositories.UserValidator", 
                    true,
                    BindingFlags.CreateInstance, 
                    null, 
                    new object[] { new DateTime(2017, 1, 1), ".*" },
                    CultureInfo.CurrentCulture, 
                    new object[0]);
                var repository = domain.CreateInstanceAndUnwrap(
                    "Repositories", 
                    this._userRepositories[service.Repository].FullName, 
                    true,
                    BindingFlags.CreateInstance, 
                    null, 
                    new object[] { validator, idGenerator },
                    CultureInfo.CurrentCulture, 
                    new object[0]);

                if (service.IsMaster)
                { 
                    if (master != null)
                    {
                        throw new ConfigurationErrorsException("Two ore more master-services.");
                    }  
                                         
                    if (point != null)
                    {
                        masterPoints.Add(point);
                    }  
                                     
                    master = domain.CreateInstanceAndUnwrap(
                        "Services", 
                        "Services.MasterUserService", 
                        true,
                        BindingFlags.CreateInstance, 
                        null, 
                        new object[] { service.Id, slavePoints.Values.ToList(), masterPoints, repository, logger },
                        CultureInfo.CurrentCulture, 
                        new object[0]) as IUserService;
                }
                else
                {
                    slaves.Add(
                        domain.CreateInstanceAndUnwrap(
                            "Services", 
                            "Services.SlaveUserService", 
                            true,
                            BindingFlags.CreateInstance, 
                            null, 
                            new object[] { service.Id, point, repository, logger },
                            CultureInfo.CurrentCulture, new object[0]) as IUserService);
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
