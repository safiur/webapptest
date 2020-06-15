using Attestation.Event.Subscriber.BL.ServiceClasses;
using Attestation.Event.Subscriber.BL.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Attestation.Event.Subscriber.BL
{
    /// <summary>
    /// class IServiceCollection extenstion
    /// </summary>
    public static class ServiceCollectionExtension
    {

        /// <summary>
        /// Extension method for IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IAttestationMessageProcessingService, AttestationMessageProcessingService>();
            return services;
        }
    }
}
