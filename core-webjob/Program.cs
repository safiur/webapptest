using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Samples.WebJobs.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
            .ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddTimers();
            })
            .UseConsoleLifetime()         
            .ConfigureServices((context, service) =>
             {
                 service.AddTransient<SampleScheduledWebJob, SampleScheduledWebJob>();
                 service.AddTransient<IUsefulRepository, UsefulRepository>();
             });

            var host = builder.Build();
            using (host)
            {
                host.Run();
            }
        }
    }
}