using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apis.Common.Asp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmeData.FTI.WebApi.Services;

namespace SmeData.FTI.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // services.AddLogging(opt => opt.AddSerilog());
            var ftiPath = this.Configuration.GetValue<string>("FtiPath");
            var docClassifierPath = this.Configuration.GetValue<string>("DocClassifierPath");
            var resultsGrouperPath = this.Configuration.GetValue<string>("BaseActsJson");

            services.AddSingleton<ISearchService>(new SearchService(ftiPath, docClassifierPath, resultsGrouperPath));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseApisExceptionMiddleware();
            app.UseMvc();
            
        }
        private static void ConfigureLoggerService(IConfiguration configuration)
        {
            // the logger usage is registered in Program.cs -> BuildWebHost -> UseSerilog()

            
        }
    }
}
