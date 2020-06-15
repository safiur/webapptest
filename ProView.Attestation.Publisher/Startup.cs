using System;
using System.Data;
using System.Data.SqlClient;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProView.Attestation.Publisher.Infrastructure;
using Serilog;
using Serilog.Context;

namespace ProView.Attestation.Publisher
{
    /// <summary>
    /// Class that manage dependency and middleware
    /// </summary>
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks().AddAzureServiceBusTopic(
                _configuration[AppConstants.AzureSettingsServiceBusConnectionString],
                topicName: _configuration[AppConstants.AzureSettingsServiceBusTopic],
                name: "attestation-servicebus-check",
                tags: new string[] { "Service Bus" }
            );

            //services.AddHealthChecksUI();
            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<IConfiguration>(this._configuration);
            services.AddSwaggerDocumentation();
            services.RegisterServices(_configuration);
            services.AddTransient<IDbConnection>(db => new SqlConnection(
                _configuration.GetConnectionString(AppConstants.ConnectionStringsPrPubDefaultConnectionString)));
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the apiKey scheme. Example: \"Authorization: {apiKey}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.DocumentFilter<BasicAuthDocumentFilter>();
            });
        }



        /// <summary>
        /// Method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            loggerFactory.AddSerilog();
            loggerFactory.AddFile(AppConstants.LoggerTracePath, LogLevel.Trace);
            loggerFactory.AddFile(AppConstants.LoggerErrorPath, LogLevel.Error);

            app.Use(async (httpContext, next) =>
            {
                //Get remote IP address  
                var ip = httpContext.Connection.RemoteIpAddress.ToString();
                LogContext.PushProperty("IP", !String.IsNullOrWhiteSpace(ip) ? ip : "unknown");

                await next.Invoke();
            });
            //app.UseHealthChecksUI();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseSwaggerDocumentation();
            app.UseMvc();
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
