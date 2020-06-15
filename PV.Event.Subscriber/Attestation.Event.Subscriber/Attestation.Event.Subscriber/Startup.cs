using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Attestation.Event.Subscriber.BL.ServiceClasses;
using Attestation.Event.Subscriber.BL.ServiceContracts;
using PV.Event.Constants;
using Serilog;

namespace Attestation.Event.Subscriber
{
    /// <summary>
    /// Class that manage dependency and middleware
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: false, reloadOnChange: true)
               // .AddJsonFile("azurekeyvault.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var config = builder.Build();

            //builder.AddAzureKeyVault(
            //    $"https://{config["azureKeyVault:vault"]}.vault.azure.net/",
            //    config["azureKeyVault:clientId"],
            //    config["azureKeyVault:clientSecret"]
            //);

            _configuration = builder.Build();
        }

        /// <summary>
        /// Method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddScoped(typeof(DAL.RepositoryContracts.IMessageLogRepository<>), typeof(DAL.RepositoryClasses.MessageLogRepository<>));
            services.AddScoped<IAttestationMessageProcessingService, AttestationMessageProcessingService>();

            services.AddHealthChecks().AddAzureServiceBusTopic(
                _configuration[AppConstants.AzureSettingsServiceBusConnectionString],
                topicName: _configuration[AppConstants.AzureSettingsServiceBusTopic],
                name: "attestation-servicebus-topic",
                tags: new string[] { "Service Bus" }
            )
                .AddSqlServer(
                    _configuration[AppConstants.ConnectionStringsDefaultConnectionString],
                    name: "Database: PVEventMessagingSvc_SIT3",
                    tags: new string[] { "DataBase" });
            //services.AddHealthChecksUI();
            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        /// <param name="loggerFactory">ILoggerFactory</param>
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddSerilog();
            //app.UseHealthChecksUI();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            //app.UseMvc();

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Subscriber App is running");
            });
        }
    }
}