using Attestation.Event.Subscriber.DAL.Entities;
using Attestation.Event.Subscriber.DAL.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Attestation.Event.Subscriber.DAL
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IMessageLogRepository<AttestationMessage>), typeof(IMessageLogRepository<AttestationMessage>));
            return services;
        }
    }
}
