using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using CountyResidenceCredentialsIssuer.Data;
using CountyResidenceCredentialsIssuer.MattrOpenApiClient;
using CountyResidenceCredentialsIssuer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CountyResidenceCredentialsIssuer
{
    public class MattrCredentialsService
    {
        private readonly IConfiguration _configuration;
        private readonly CountyResidenceCredentialsIssuerCredentialsService _CountyPermitResidenceCredentialsIssuerCredentialsService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;
        private readonly MattrConfiguration _mattrConfiguration;

        public MattrCredentialsService(IConfiguration configuration,
            CountyResidenceCredentialsIssuerCredentialsService CountyPermitResidenceCredentialsIssuerCredentialsService,
            IHttpClientFactory clientFactory,
            IOptions<MattrConfiguration> mattrConfiguration,
            MattrTokenApiService mattrTokenApiService)
        {
            _configuration = configuration;
            _CountyPermitResidenceCredentialsIssuerCredentialsService = CountyPermitResidenceCredentialsIssuerCredentialsService;
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
            _mattrConfiguration = mattrConfiguration.Value;
        }

        public async Task<string> CreateCredentialsAndCallback(string name)
        {
            // create a new one
            var countyResidenceDataCredentials = await CreateMattrDidAndCredentialIssuer();
            countyResidenceDataCredentials.Name = name;
            await _CountyPermitResidenceCredentialsIssuerCredentialsService.CreateCountyResidenceData(countyResidenceDataCredentials);

            var callback = $"https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers/{countyResidenceDataCredentials.OidcIssuerId}/federated/callback";
            return callback;
        }

        private async Task<CountyResidenceDataCredentials> CreateMattrDidAndCredentialIssuer()
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var did = await CreateMattrDid(client);
            var oidcIssuer = await CreateMattrCredentialIssuer(client, did);

            return new CountyResidenceDataCredentials
            {
                Name = "not_named",
                Did = JsonConvert.SerializeObject(did),
                OidcIssuer = JsonConvert.SerializeObject(oidcIssuer),
                OidcIssuerId = oidcIssuer.Id
            };
        }

        private async Task<V1_CreateOidcIssuerResponse> CreateMattrCredentialIssuer(HttpClient client, V1_CreateDidResponse did)
        {
            // create vc, post to credentials api
            // https://learn.mattr.global/tutorials/issue/oidc-bridge/setup-issuer

            var createCredentialsUrl = $"https://{_mattrConfiguration.TenantSubdomain}/ext/oidc/v1/issuers";

            var payload = new MattrOpenApiClient.V1_CreateOidcIssuerRequest
            {
                Credential = new Credential
                {
                    IssuerDid = did.Did,
                    Name = "CountyResidence",
                    Context = new List<Uri> {
                        new Uri( "https://schema.org"),
                        new Uri( "https://www.w3.org/2018/credentials/v1")
                    },
                    Type = new List<string> { "VerifiableCredential" }
                },
                ClaimMappings = new List<ClaimMappings>
                {
                    new ClaimMappings{ JsonLdTerm="family_name", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/family_name"},
                    new ClaimMappings{ JsonLdTerm="given_name", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/given_name"},
                    new ClaimMappings{ JsonLdTerm="date_of_birth", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/date_of_birth"},
                    new ClaimMappings{ JsonLdTerm="address_country", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/address_country"},
                    new ClaimMappings{ JsonLdTerm="address_locality", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/address_locality"},
                    new ClaimMappings{ JsonLdTerm="address_region", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/address_region"},
                    new ClaimMappings{ JsonLdTerm="street_address", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/street_address"},
                    new ClaimMappings{ JsonLdTerm="postal_code", OidcClaim=$"https://{_mattrConfiguration.TenantSubdomain}/postal_code"}
                },
                FederatedProvider = new FederatedProvider
                {
                    ClientId = _configuration["Auth0Wallet:ClientId"],
                    ClientSecret = _configuration["Auth0Wallet:ClientSecret"],
                    Url = new Uri($"https://{_configuration["Auth0Wallet:Domain"]}"),
                    Scope = new List<string> { "openid", "profile", "email" }
                }
            };

            var payloadJson = JsonConvert.SerializeObject(payload);

            var uri = new Uri(createCredentialsUrl);

            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var createOidcIssuerResponse = await client.PostAsync(uri, content);

                if (createOidcIssuerResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var v1CreateOidcIssuerResponse = JsonConvert.DeserializeObject<V1_CreateOidcIssuerResponse>(
                            await createOidcIssuerResponse.Content.ReadAsStringAsync());

                    return v1CreateOidcIssuerResponse;
                }

                var error = await createOidcIssuerResponse.Content.ReadAsStringAsync();
            }

            throw new Exception("whoops something went wrong");
        }

        private async Task<V1_CreateDidResponse> CreateMattrDid(HttpClient client)
        {
            // create did , post to dids 
            // https://learn.mattr.global/api-ref/#operation/createDid
            // https://learn.mattr.global/tutorials/dids/use-did/

            var createDidUrl = $"https://{_mattrConfiguration.TenantSubdomain}/core/v1/dids";

            var payload = new MattrOpenApiClient.V1_CreateDidDocument
            {
                Method = MattrOpenApiClient.V1_CreateDidDocumentMethod.Key,
                Options = new MattrOptions()
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var uri = new Uri(createDidUrl);

            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var createDidResponse = await client.PostAsync(uri, content);

                if (createDidResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var v1CreateDidResponse = JsonConvert.DeserializeObject<V1_CreateDidResponse>(
                            await createDidResponse.Content.ReadAsStringAsync());

                    return v1CreateDidResponse;
                }

                var error = await createDidResponse.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
    public class MattrOptions
    {
        /// <summary>
        /// The supported key types for the DIDs are ed25519 and bls12381g2. 
        /// If the keyType is omitted, the default key type that will be used is ed25519.
        /// 
        /// If the keyType in options is set to bls12381g2 a DID will be created with 
        /// a BLS key type which supports BBS+ signatures for issuing ZKP-enabled credentials.
        /// </summary>
        public string keyType { get; set; } = "bls12381g2";
    }
}