using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Misc.SAPIntegration.Services;

namespace Nop.Plugin.Misc.SAPIntegration.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<SAPIntegrationService>().As<ISAPIntegrationService>().InstancePerLifetimeScope();

            //builder.RegisterType<CustomModelFactory>().As<ICustomModelFactory>().InstancePerLifetimeScope();
        }

        public int Order => 11;
    }
}
