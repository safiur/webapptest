using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProView.Attestation.Publisher.BL.ServiceClasses;
using ProView.Attestation.Publisher.BL.ServiceContracts;
using ProView.Attestation.Publisher.DAL.Entities;
using ProView.Attestation.Publisher.DAL.RepositoryClasses;
using ProView.Attestation.Publisher.DAL.RepositoryContracts;

namespace ProView.Attestation.Publisher.Infrastructure
{
    /// <summary>
    /// Class to add dependency injection
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Method to register dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<IAttestationPublisherService, AttestationPublisherService>();
            services.AddTransient(typeof(IMessageLogRepository<FailedAttestationMessage>),
                typeof(MessageLogRepository<FailedAttestationMessage>));
            services.AddTransient<IMessageLogService, MessageLogService>();
        }
    }
}
