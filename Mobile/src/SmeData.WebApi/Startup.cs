using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apis.Common.Asp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Services;
using SmeData.WebApi.Services.DecisionSupport;
using SmeData.WebApi.Services.Documents;
using SmeData.WebApi.Services.EuCaselawFilter;
using SmeData.WebApi.Services.Searches;
using SmeData.WebApi.Services.Sort;
using SmeData.WebApi.Services.Values;

namespace SmeData.WebApi
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
            this.ConfigureDbContext(services, this.Configuration);
            services.AddHttpClient<ISearchService, SearchService>(client =>
            {
                string apiUrl = this.Configuration.GetSection("SearchApiUrl").Value;
                client.BaseAddress = new Uri(apiUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AngularOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //builder.WithOrigins(clientAppUrl).AllowAnyHeader());
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AngularOrigin"));
            });
        }

        private void ConfigureDbContext(IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("EuCases");
            var settings = new EuCasesContextFactorySettings(connectionString);
            var pathsProvider = new PathsProvider()
            {
                BasePath = config.GetValue<string>("BasePath"),
                PdfPath = config.GetValue<string>("PdfPath"),
            };
            services.AddSingleton<IPathsProvider>(pathsProvider);
            services.AddSingleton(settings);
            services.AddSingleton<IEuCasesContextFactory, EucasesContextFactory>();
            services.AddScoped<IDocumentsService, DocumentsService>();
            services.AddScoped<IDecisionSupportService, DecisionSupportService>();
            services.AddSingleton<IValuesService, ValuesService>();
            services.AddSingleton<ISortService, SortService>();
            services.AddSingleton<IEuCaselawFilterService, EuCaselawFilterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseApisExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseCors("AngularOrigin");
            //app.UseResponseCompression();

            app.UseMvc();

        }
    }
}
