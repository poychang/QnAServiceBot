using Autofac;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule.Interface;
using QnAMakerBot.AgentModule.Scorable;

namespace QnAMakerBot.AgentModule
{
    /// <summary>註冊至 AutoFac 容器的模組</summary>
    public class AgentModuleLoader : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<InMemoryAgentStore>()
                .Keyed<IAgentProvider>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AgentService>()
                .Keyed<IAgentService>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<AgentToUserRouter>()
               .Keyed<IAgentToUser>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserToAgentRouter>()
                .Keyed<IUserToAgent>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<BotStateMappingStorage>()
               .Keyed<IAgentUserMapping>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder.RegisterType<AgentToUserScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserToAgentScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
        }
    }
}
