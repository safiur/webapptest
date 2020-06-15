using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ProView.Attestation.Publisher.Infrastructure
{
    /// <summary>
    /// Swagger Service Extensions
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Method to add Swagger Documentation
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo() { Title = "Proview Event Messaging API v1.0", Version = "v1.0" });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "PV.API.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Method to Use Swagger Documentation
        /// </summary>
        /// <param name="app"></param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");
                c.DocumentTitle = "ProView | Event Messaging API Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }



    }

    /// <summary>
    /// Swagger document filter
    /// </summary>
    public class BasicAuthDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirements = new Dictionary<string, IEnumerable<string>>()
            {
                { "Bearer", new string[] { } }  // in swagger you specify empty list unless using OAuth2 scopes
            };

            swaggerDoc.SecurityRequirements = new[] { new OpenApiSecurityRequirement {{
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basicAuth" }
                },
                new string[]{}
            }},  };
        }
    }
}
