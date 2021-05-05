using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Factories;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Services;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ProductRibbonService>().As<IProductRibbonService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRibbonModelFactory>().As<IProductRibbonModelFactory>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}
