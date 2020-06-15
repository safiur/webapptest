using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ProView.Attestation.Publisher
{
    /// <summary>
    /// Class that host and run the application
    /// </summary>
    public class Program
    { /// <summary>
        /// Configuration object member
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

        /// <summary>
        /// Main method: entry point of application
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
            Log.CloseAndFlush();
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
