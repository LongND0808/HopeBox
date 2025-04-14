using Duende.IdentityServer.Models;
using HopeBox.Core.Email;

namespace HopeBox.Core.Config
{
    public interface IConfig
    {
        IEnumerable<IdentityResource> GetIdentityResources();
        IEnumerable<ApiScope> GetApiScopes();
        IEnumerable<Client> GetClients();
        EmailConfiguration GetEmailConfiguration();
    }
}
