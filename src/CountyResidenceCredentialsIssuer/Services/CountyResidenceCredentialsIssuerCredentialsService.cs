using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CountyResidenceCredentialsIssuer.Data;
using CountyResidenceCredentialsIssuer.MattrOpenApiClient;
using CountyResidenceCredentialsIssuer.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CountyResidenceCredentialsIssuer
{
    public class CountyResidenceCredentialsIssuerCredentialsService
    {
        private readonly CountyResidenceDataMattrContext _countyResidenceDataMattrContext;
        private readonly MattrConfiguration _mattrConfiguration;

        public CountyResidenceCredentialsIssuerCredentialsService(CountyResidenceDataMattrContext countyResidenceDataMattrContext,
            IOptions<MattrConfiguration> mattrConfiguration)
        {
            _countyResidenceDataMattrContext = countyResidenceDataMattrContext;
            _mattrConfiguration = mattrConfiguration.Value;
        }

        public async Task<(string Callback, string DidId)> GetLastCountyResidenceCredentialIssuer()
        {
            var countyResidenceDataCredentials = await _countyResidenceDataMattrContext
                .CountyResidenceDataCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (countyResidenceDataCredentials != null)
            {
                var callback = $"https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{countyResidenceDataCredentials.OidcIssuerId}/federated/callback";
                var oidcCredentialIssuer = JsonConvert.DeserializeObject<V1_CreateOidcIssuerResponse>(countyResidenceDataCredentials.OidcIssuer);
                return (callback, oidcCredentialIssuer.Credential.IssuerDid);
            }

            return (string.Empty, string.Empty);
        }

        public async Task<string> GetLastCountyResidenceDataCredentialIssuerUrl()
        {
            var countyResidenceData = await _countyResidenceDataMattrContext
                .CountyResidenceDataCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (countyResidenceData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{countyResidenceData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task<string> GetCountyResidenceDataCredentialIssuerUrl(string name)
        {
            var countyResidenceData = await _countyResidenceDataMattrContext
                .CountyResidenceDataCredentials
                .FirstOrDefaultAsync(dl => dl.Name == name);

            if (countyResidenceData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{countyResidenceData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task CreateCountyResidenceData(CountyResidenceDataCredentials countyResidenceDataCredentials)
        {
            _countyResidenceDataMattrContext.CountyResidenceDataCredentials.Add(countyResidenceDataCredentials);
            await _countyResidenceDataMattrContext.SaveChangesAsync();
        }
    }
}
