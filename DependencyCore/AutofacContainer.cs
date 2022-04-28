using Autofac;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace DependencyCore
{
    public static class AutofacContainer
    {
        private static IContainer _container = null!;

        public static void InitContainer(IConfiguration configuration)
        {
            _container = BuildContainer(configuration);
        }

        public static IContainer GetContainer()
        {
            if (_container is null)
                throw new NullReferenceException("Container not initialized");

            return _container;
        }

        private static IContainer BuildContainer(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(configuration).As<IConfiguration>();

            InitDALRegistrations(builder);
            InitLogicRegistrations(builder);

            return builder.Build();
        }

        private static void InitLogicRegistrations(ContainerBuilder builder)
        {
        }

        private static void InitDALRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<UserDAO>().As<IUserDAO>();
        }
    }
}
