using Autofac;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Store.Domain.Context;
using Store.Extensions;
using Store.Services.Mailing;

namespace Store.Services
{
    public class ServicesModule : Module
    {
        private readonly IConfiguration configuration;
        private bool IsDevelopment => configuration.GetEnvironmentVariables().IsDevelopment;
        public ServicesModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ExtendedDbContext>()
                .As<IApplicationContext>()
                .InstancePerLifetimeScope();
                

            builder
                .RegisterType<SmtpClient>()
                .AsImplementedInterfaces();
            

            builder
               .RegisterTypes(new[] { typeof(DevelopmentEmailService), typeof(ProductionMailingService) });

            builder
                .RegisterTypes(new[] { typeof(DevelopmentEmailService), typeof(ProductionMailingService) })
                .AsImplementedInterfaces()
                .OnActivating
                (
                    args =>
                    {
                        IMailingService instance = IsDevelopment is true
                            ? args.Context.Resolve<DevelopmentEmailService>()
                            : args.Context.Resolve<ProductionMailingService>() as IMailingService;

                        args.ReplaceInstance(instance);
                    }
                )
                .InstancePerDependency();
        }
    }
}
