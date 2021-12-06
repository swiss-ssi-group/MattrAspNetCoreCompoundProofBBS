using VerifyEidAndCountyResidence.MattrOpenApiClient;
using VerifyEidAndCountyResidence.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VerifyEidAndCountyResidence.Data;
using Microsoft.Extensions.Options;

namespace VerifyEidAndCountyResidence
{
    public class MattrPresentationTemplateService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;
        private readonly VerifyEidCountyResidenceDbService _verifyEidAndCountyResidenceDbService;
        private readonly MattrConfiguration _mattrConfiguration;

        public MattrPresentationTemplateService(IHttpClientFactory clientFactory,
            IOptions<MattrConfiguration> mattrConfiguration,
            MattrTokenApiService mattrTokenApiService,
            VerifyEidCountyResidenceDbService VerifyEidAndCountyResidenceDbService)
        {
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
            _verifyEidAndCountyResidenceDbService = VerifyEidAndCountyResidenceDbService;
            _mattrConfiguration = mattrConfiguration.Value;
        }

        public async Task<string> CreatePresentationTemplateId(string didEid, string didCountyResidence)
        {
            // create a new one
            var v1PresentationTemplateResponse = await CreateMattrPresentationTemplate(didEid, didCountyResidence);

            // save to db
            var template = new EidCountyResidenceDataPresentationTemplate
            {
                DidEid = didEid,
                DidCountyResidence = didCountyResidence,
                TemplateId = v1PresentationTemplateResponse.Id,
                MattrPresentationTemplateReponse = JsonConvert.SerializeObject(v1PresentationTemplateResponse)
            };
            await _verifyEidAndCountyResidenceDbService.CreateEidAndCountyResidenceDataTemplate(template);

            return v1PresentationTemplateResponse.Id;
        }

        private async Task<V1_PresentationTemplateResponse> CreateMattrPresentationTemplate(string didId, string didCountyResidence)
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var v1PresentationTemplateResponse = await CreateMattrPresentationTemplate(client, didId, didCountyResidence);
            return v1PresentationTemplateResponse;
        }

        private async Task<V1_PresentationTemplateResponse> CreateMattrPresentationTemplate(
            HttpClient client, string didEid, string didCountyResidence)
        {
            // create presentation, post to presentations templates api
            // https://learn.mattr.global/tutorials/verify/presentation-request-template
            // https://learn.mattr.global/tutorials/verify/presentation-request-template#create-a-privacy-preserving-presentation-request-template-for-zkp-enabled-credentials

            var createPresentationsTemplatesUrl = $"https://{_mattrConfiguration.TenantSubdomain}/v1/presentations/templates";

            var eidAdditionalPropertiesCredentialSubject = new Dictionary<string, object>();
            eidAdditionalPropertiesCredentialSubject.Add("credentialSubject", new EidDataCredentialSubject
            {
                Explicit = true
            });

            var countyResidenceAdditionalPropertiesCredentialSubject = new Dictionary<string, object>();
            countyResidenceAdditionalPropertiesCredentialSubject.Add("credentialSubject", new CountyResidenceDataCredentialSubject
            {
                Explicit = true
            });
            

            var additionalPropertiesCredentialQuery = new Dictionary<string, object>();
            additionalPropertiesCredentialQuery.Add("required", true);

            var additionalPropertiesQuery = new Dictionary<string, object>();
            additionalPropertiesQuery.Add("type", "QueryByFrame");
            additionalPropertiesQuery.Add("credentialQuery", new List<CredentialQuery2> {
                new CredentialQuery2
                {
                    Reason = "Please provide your E-ID",
                    TrustedIssuer = new List<TrustedIssuer>{
                        new TrustedIssuer
                        {
                            Required = true,
                            Issuer = didEid // DID used to create the oidc
                        }
                    },
                    Frame = new Frame
                    {
                        Context = new List<object>{
                            "https://www.w3.org/2018/credentials/v1",
                            "https://w3c-ccg.github.io/ldp-bbs2020/context/v1",
                            "https://schema.org",
                        },
                        Type = "VerifiableCredential",
                        AdditionalProperties = eidAdditionalPropertiesCredentialSubject

                    },
                    AdditionalProperties = additionalPropertiesCredentialQuery
                },
                new CredentialQuery2
                {
                    Reason = "Please provide your Residence data",
                    TrustedIssuer = new List<TrustedIssuer>{
                        new TrustedIssuer
                        {
                            Required = true,
                            Issuer = didCountyResidence // DID used to create the oidc
                        }
                    },
                    Frame = new Frame
                    {
                        Context = new List<object>{
                            "https://www.w3.org/2018/credentials/v1",
                            "https://w3c-ccg.github.io/ldp-bbs2020/context/v1",
                            "https://schema.org",
                        },
                        Type = "VerifiableCredential",
                        AdditionalProperties = countyResidenceAdditionalPropertiesCredentialSubject

                    },
                    AdditionalProperties = additionalPropertiesCredentialQuery
                }
            });

            var payload = new MattrOpenApiClient.V1_CreatePresentationTemplate
            {
                Domain = _mattrConfiguration.TenantSubdomain,
                Name = "zkp-eid-county-residence-2",
                Query = new List<Query>
                {
                    new Query
                    {
                        AdditionalProperties = additionalPropertiesQuery
                    }
                }
            };

            var payloadJson = JsonConvert.SerializeObject(payload);

            var uri = new Uri(createPresentationsTemplatesUrl);
           
            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var presentationTemplateResponse = await client.PostAsync(uri, content);

                if (presentationTemplateResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {

                    var v1PresentationTemplateResponse = JsonConvert
                            .DeserializeObject<MattrOpenApiClient.V1_PresentationTemplateResponse>(
                            await presentationTemplateResponse.Content.ReadAsStringAsync());

                    return v1PresentationTemplateResponse;
                }

                var error = await presentationTemplateResponse.Content.ReadAsStringAsync();

            }

            throw new Exception("whoops something went wrong");
        }
    }

    public class EidDataCredentialSubject
    {
        [Newtonsoft.Json.JsonProperty("@explicit", Required = Newtonsoft.Json.Required.Always)]
        public bool Explicit { get; set; }

        // Common
        [Newtonsoft.Json.JsonProperty("date_of_birth", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object DateOfBirth { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("family_name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object FamilyName { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("given_name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object GivenName { get; set; } = new object();

        // E-ID

        [Newtonsoft.Json.JsonProperty("birth_place", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object BirthPlace { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("height", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object Height { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("nationality", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object Nationality { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("gender", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object Gender { get; set; } = new object();
    }

    public class CountyResidenceDataCredentialSubject
    {
        [Newtonsoft.Json.JsonProperty("@explicit", Required = Newtonsoft.Json.Required.Always)]
        public bool Explicit { get; set; }

        // Common
        [Newtonsoft.Json.JsonProperty("date_of_birth", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object DateOfBirth { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("family_name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object FamilyName { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("given_name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object GivenName { get; set; } = new object();

        // section Country Residence
        [Newtonsoft.Json.JsonProperty("address_country", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object AddressCountry { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("address_locality", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object AddressLocality { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("address_region", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object AddressRegion { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("street_address", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object StreetAddress { get; set; } = new object();

        [Newtonsoft.Json.JsonProperty("postal_code", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public object PostalCode { get; set; } = new object();
    }
}