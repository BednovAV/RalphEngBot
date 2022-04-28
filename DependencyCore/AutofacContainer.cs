using AuthenticationCore;
using Autofac;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Entities;
using LogicLayer.StateStrategy;
using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;

namespace DependencyCore
{
    public static class AutofacContainer
    {
        private static IContainer _container = null!;

        public static void InitContainer(IConfiguration configuration, ITelegramBotClient botClient)
        {
            _container = BuildContainer(configuration, botClient);
        }

        public static IContainer GetContainer()
        {
            if (_container is null)
                throw new NullReferenceException("Container not initialized");

            return _container;
        }

        private static IContainer BuildContainer(IConfiguration configuration, ITelegramBotClient botClient)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(configuration).As<IConfiguration>();
            builder.RegisterInstance(botClient).As<ITelegramBotClient>();

            InitDALRegistrations(builder);
            InitLogicRegistrations(builder);
            InitCoreRegistrations(builder);

            return builder.Build();
        }

        private static void InitCoreRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticateCore>().As<IAuthenticationCore>();
        }
        
        private static void InitLogicRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<WaitingCommandStrategy>().Keyed<IStateStrategy>(WaitingCommandStrategy.State);
            builder.RegisterType<WaitingNewNameStrategy>().Keyed<IStateStrategy>(WaitingNewNameStrategy.State);
        }

        private static void InitDALRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<UserDAO>().As<IUserDAO>();
        }
    }
}
