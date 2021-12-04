using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CountyPermitResidenceCredentialsIssuer.Data;
using CountyPermitResidenceCredentialsIssuer.MattrOpenApiClient;
using CountyPermitResidenceCredentialsIssuer.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CountyPermitResidenceCredentialsIssuer
{
    public class CountyPermitResidenceCredentialsIssuerCredentialsService
    {
        private readonly CountyResidenceDataMattrContext _eidDataMattrContext;
        private readonly MattrConfiguration _mattrConfiguration;

        public CountyPermitResidenceCredentialsIssuerCredentialsService(CountyResidenceDataMattrContext eidDataMattrContext,
            IOptions<MattrConfiguration> mattrConfiguration)
        {
            _eidDataMattrContext = eidDataMattrContext;
            _mattrConfiguration = mattrConfiguration.Value;
        }

        public async Task<(string Callback, string DidId)> GetLastEidCredentialIssuer()
        {
            var eidDataCredentials = await _eidDataMattrContext
                .EidDataCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (eidDataCredentials != null)
            {
                var callback = $"https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{eidDataCredentials.OidcIssuerId}/federated/callback";
                var oidcCredentialIssuer = JsonConvert.DeserializeObject<V1_CreateOidcIssuerResponse>(eidDataCredentials.OidcIssuer);
                return (callback, oidcCredentialIssuer.Credential.IssuerDid);
            }

            return (string.Empty, string.Empty);
        }

        public async Task<string> GetLastEidDataCredentialIssuerUrl()
        {
            var eidData = await _eidDataMattrContext
                .EidDataCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (eidData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{eidData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task<string> GetEidDataCredentialIssuerUrl(string name)
        {
            var eidData = await _eidDataMattrContext
                .EidDataCredentials
                .FirstOrDefaultAsync(dl => dl.Name == name);

            if (eidData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{eidData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task CreateEidData(CountyResidenceDataCredentials eidDataCredentials)
        {
            _eidDataMattrContext.EidDataCredentials.Add(eidDataCredentials);
            await _eidDataMattrContext.SaveChangesAsync();
        }
    }
}
