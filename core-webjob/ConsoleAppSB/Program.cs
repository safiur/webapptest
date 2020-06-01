using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConsoleAppSB.Interfaces;
using ConsoleAppSB.Services;
using Microsoft.Azure.WebJobs.Host;

namespace ConsoleAppSB
{
	class Program
	{
		static void Main(string[] args)
		{
			HostBuilder builder = new HostBuilder();

			//Below piece of code is not required if name of json file is 'appsettings.json'
			builder.ConfigureAppConfiguration(c =>
		   {
			   c.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			   c.AddEnvironmentVariables();
		   })
			.ConfigureWebJobs(b =>
			{
				b.AddAzureStorageCoreServices()
				.AddAzureStorage()
				//https://github.com/Azure/azure-webjobs-sdk/blob/554b7ba922be3a4e1f380034dc0c62d4efb2aa79/sample/SampleHost/Program.cs#L20
				.AddServiceBus();
			})
			.ConfigureLogging((context, b) =>
			{
				b.AddConsole();
			})
			.ConfigureServices(s =>
		   {
			   s.AddSingleton<IDataStore, DataStore>();
		   });

			using (var host = builder.Build())
			{
				host.Run();
			}
		}
	}
}
