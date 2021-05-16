using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
        
        }


        public int Order => 11;

    }
}
