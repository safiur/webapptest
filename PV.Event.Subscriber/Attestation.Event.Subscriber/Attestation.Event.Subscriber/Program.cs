
using System;
using Attestation.Event.Subscriber.BL.ServiceClasses;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using PV.Event.Constants;
using Serilog;


namespace Attestation.Event.Subscriber
{
    /// <summary>
    /// Class that host and run the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configuration object member
        /// </summary>
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Main method: entry point of application
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static void Main(string[] args)
        {


            try
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .AddEnvironmentVariables();

                var config = builder.Build();

                //builder.AddAzureKeyVault(
                //    $"https://{config["azureKeyVault:vault"]}.vault.azure.net/",
                //    config["azureKeyVault:clientId"],
                //    config["azureKeyVault:clientSecret"]
                //);

                Configuration = builder.Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithThreadId()
                    .CreateLogger();

                ProcessAttestationMessage.SubscriptionClient = new SubscriptionClient(
                    Configuration[AppConstants.AzureSettingsServiceBusConnectionString]
                    , Configuration[AppConstants.AzureSettingsServiceBusTopic]
                    , Configuration[AppConstants.AzureSettingsAtEvtSubSubscriptionName]);

                ProcessAttestationMessage.Config = Configuration;
                // Register subscription message handler and receive messages in a loop
                ProcessAttestationMessage.RegisterOnMessageHandlerAndReceiveMessages();
                CreateWebHostBuilder(args).Build().Run();
            }

            catch (Exception ex)
            {
                Log.Logger.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// Method to create web host builder object
        /// </summary>
        /// <param name="args">command line arguments supplied through main method</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();

        
    }
}
