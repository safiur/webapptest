using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace PV.Event.Security
{
    /// <summary>
    /// Basic Authentication handler implementation class
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly IConfiguration _config;

        public BasicAuthenticationHandler(IConfiguration config,IOptionsMonitor<AuthenticationSchemeOptions> options,ILoggerFactory logger,UrlEncoder encoder,ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        private string SecuredKey { get; set; }

        /// <summary>
        /// Override Implementation of HandleAuthenticateAsync
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var isIsAuthorized = false;
            SecuredKey = string.Empty;

            //if (Request.Path.HasValue && Request.Path.Value.ToLower().Contains("api"))
            //{
            //    if (!Request.Headers.ContainsKey("Authorization"))
            //        return Task.FromResult(AuthenticateResult.NoResult());

            //    try
            //    {
            //        var authHeader = Request.Headers["Authorization"][0];

            //        if (authHeader != null)
            //        {
            //            isIsAuthorized = IsAuthorize(authHeader);
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        return Task.FromResult(AuthenticateResult.NoResult());
            //    }

            //    if (!isIsAuthorized)
            //    {
            //        Log.Logger.Debug("Authorization Failed");
            //        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            //    }
            //}

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, SecuredKey),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Log.Logger.Debug("Authorization Successful");


            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        /// <summary>
        /// Check if KEy supplied in header is valid
        /// </summary>
        /// <param name="authHeaderScheme"></param>
        /// <returns></returns>
        private bool IsAuthorize(string authHeaderScheme)
        {
            var keyValuePairs = GetApiKeys();
            SecuredKey = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderScheme));
            foreach (var valuePair in keyValuePairs)
            {
                if (valuePair.Value == SecuredKey)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get Api keys from appsettings.json
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, string>> GetApiKeys()
        {
            var myArraySection = _config.GetSection("ApiKeys");
            var itemArray = myArraySection.AsEnumerable();
            return itemArray;
        }

        
    }


}