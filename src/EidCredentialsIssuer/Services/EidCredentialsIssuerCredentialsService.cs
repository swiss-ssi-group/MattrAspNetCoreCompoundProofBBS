using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EidCredentialsIssuer.Data;
using EidCredentialsIssuer.MattrOpenApiClient;
using EidCredentialsIssuer.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace EidCredentialsIssuer
{
    public class EidCredentialsIssuerCredentialsService
    {
        private readonly EidDataMattrContext _eidDataMattrContext;
        private readonly MattrConfiguration _mattrConfiguration;

        public EidCredentialsIssuerCredentialsService(EidDataMattrContext eidDataMattrContext,
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
            var vaccinationData = await _eidDataMattrContext
                .EidDataCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (vaccinationData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{vaccinationData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task<string> GetEidDataCredentialIssuerUrl(string name)
        {
            var vaccinationData = await _eidDataMattrContext
                .EidDataCredentials
                .FirstOrDefaultAsync(dl => dl.Name == name);

            if (vaccinationData != null)
            {
                var url = $"openid://discovery?issuer=https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{vaccinationData.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task CreateEidData(EidDataCredentials eidDataCredentials)
        {
            _eidDataMattrContext.EidDataCredentials.Add(eidDataCredentials);
            await _eidDataMattrContext.SaveChangesAsync();
        }
    }
}
