using Duende.IdentityServer.Models;
using HopeBox.Core.Email;
using Microsoft.Extensions.Configuration;

namespace HopeBox.Core.Config
{
    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            var identityResources = _configuration.GetSection("IdentityServer:IdentityResources")
                .Get<List<IdentityResource>>();

            return identityResources ?? throw new InvalidOperationException("No Identity Resources found.");
        }

        public IEnumerable<ApiScope> GetApiScopes()
        {
            var apiScopes = _configuration.GetSection("IdentityServer:ApiScopes")
                .Get<List<ApiScope>>();

            return apiScopes ?? throw new InvalidOperationException("No API Scopes found.");
        }

        public IEnumerable<Client> GetClients()
        {
            var clients = _configuration.GetSection("IdentityServer:Clients")
                .Get<List<Client>>();

            return clients ?? throw new InvalidOperationException("No Clients found.");
        }

        public EmailConfiguration GetEmailConfiguration()
        {
            var emailConfiguration = _configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            return emailConfiguration ?? throw new InvalidOperationException("No Email Configuration found.");
        }
    }
}
