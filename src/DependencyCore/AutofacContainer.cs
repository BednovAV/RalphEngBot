using AuthenticationCore;
using Autofac;
using Communication;
using Communication.MessageReceivers.LearnGrammar;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Grammar;
using LogicLayer.Interfaces.Words;
using LogicLayer.Services;
using LogicLayer.Services.Grammar;
using LogicLayer.Services.Grammar.MessageGenerators;
using LogicLayer.Services.Words;
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
            InitMessagesRegistrations(builder);
            InitCoreRegistrations(builder);

            return builder.Build();
        }

        private static void InitMessagesRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<WaitingCommandReceiver>().Keyed<IMessageReceiver>(WaitingCommandReceiver.State);
            builder.RegisterType<WaitingNewWordReceiver>().Keyed<IMessageReceiver>(WaitingNewWordReceiver.State);
            builder.RegisterType<WaitingLearnWordAnswerReceiver>().Keyed<IMessageReceiver>(WaitingLearnWordAnswerReceiver.State);
            builder.RegisterType<LearnWordsMessageReceiver>().Keyed<IMessageReceiver>(LearnWordsMessageReceiver.State);
            builder.RegisterType<LearnGrammarMessageReceiver>().Keyed<IMessageReceiver>(LearnGrammarMessageReceiver.State);
            builder.RegisterType<GrammarTestInProgressReciever>().Keyed<IMessageReceiver>(GrammarTestInProgressReciever.State);
            builder.RegisterType<WaitingRepeatWordAnswerReceiver>().Keyed<IMessageReceiver>(WaitingRepeatWordAnswerReceiver.State);

            builder.RegisterType<CallbackQuerryReciever>().As<ICallbackQuerryReciever>();
            builder.RegisterType<ChatManager>().As<IChatManager>();
        }

        private static void InitCoreRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticateCore>().As<IAuthenticationCore>();
        }
        
        private static void InitLogicRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<RepetitionWordsLogic>().As<IRepetitionWordsLogic>();
            builder.RegisterType<LearnWordsLogic>().As<ILearnWordsLogic>();
            builder.RegisterType<WordsLogicMessageGenerator>().As<IWordsLogicMessageGenerator>();

            builder.RegisterType<WordsAccessor>().As<IWordsAccessor>();
            builder.RegisterType<WordsAccessorMessageGenerator>().As<IWordsAccessorMessageGenerator>();
            
            builder.RegisterType<GrammarTestAccessor>().As<IGrammarTestAccessor>();
            builder.RegisterType<TestAccessorMessageGenerator>().As<ITestAccessorMessageGenerator>();
            builder.RegisterType<GrammarTestLogic>().As<IGrammarTestLogic>();
            builder.RegisterType<TestLogicMessageGenerator>().As<ITestLogicMessageGenerator>();
        }

        private static void InitDALRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<UserDAO>().As<IUserDAO>();
            builder.RegisterType<WordTranslationDAO>().As<IWordTranslationDAO>();
            builder.RegisterType<UserWordsDAO>().As<IUserWordsDAO>();
            builder.RegisterType<AdministrationDAO>().As<IAdministrationDAO>();
            builder.RegisterType<TestQuestionDAO>().As<ITestQuestionDAO>();
            builder.RegisterType<GrammarTestDAO>().As<IGrammarTestDAO>();
        }
    }
}
