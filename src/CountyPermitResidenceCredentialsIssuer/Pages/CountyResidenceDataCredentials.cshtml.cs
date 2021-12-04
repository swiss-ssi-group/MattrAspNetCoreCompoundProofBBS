using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using CountyPermitResidenceCredentialsIssuer.Data;
using CountyPermitResidenceCredentialsIssuer.Services;

namespace CountyPermitResidenceCredentialsIssuer.Pages
{
    public class EidDataCredentialsModel : PageModel
    {
        private readonly CountyPermitResidenceCredentialsIssuerCredentialsService _CountyPermitResidenceCredentialsIssuerCredentialsService;
        private readonly MattrConfiguration _mattrConfiguration;

        public string CountyResidenceDataMessage { get; set; } = "Loading credentials";
        public bool HasCountyResidenceData { get; set; } = false;
        public CountyResidenceData CountyResidenceData { get; set; }
        public string CredentialOfferUrl { get; set; }
        public EidDataCredentialsModel(
            CountyPermitResidenceCredentialsIssuerCredentialsService countyPermitResidenceCredentialsIssuerCredentialsService,
            IOptions<MattrConfiguration> mattrConfiguration)
        {
            _CountyPermitResidenceCredentialsIssuerCredentialsService = countyPermitResidenceCredentialsIssuerCredentialsService;
            _mattrConfiguration = mattrConfiguration.Value;
        }
        public async Task OnGetAsync()
        {

            var identityHasEidDataClaims = true;

            var familyNameClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/family_name");
            var givenNameClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/given_name");
            var dateOfBirthClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/date_of_birth");
            var addressCountryClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/addressCountry");
            var addressLocalityClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/address_locality");
            var addressRegionClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/Address_region");
            var streetAddressClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/street_address");
            var postalCodeClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/postal_code");

            if (familyNameClaim == null
                || givenNameClaim == null
                || dateOfBirthClaim == null
                || addressCountryClaim == null
                || addressLocalityClaim == null
                || addressRegionClaim == null
                || streetAddressClaim == null
                || postalCodeClaim == null)
            {
                identityHasEidDataClaims = false;
            }

            if (identityHasEidDataClaims)
            {
                CountyResidenceData = new CountyResidenceData
                {
                    FamilyName = familyNameClaim.Value,
                    GivenName = givenNameClaim.Value,
                    DateOfBirth = dateOfBirthClaim.Value,
                    AddressCountry = addressCountryClaim.Value,
                    AddressLocality = addressLocalityClaim.Value,
                    AddressRegion = addressRegionClaim.Value,
                    StreetAddress = streetAddressClaim.Value,
                    PostalCode = postalCodeClaim.Value
                };
                // get per name
                //var offerUrl = await _CountyPermitResidenceCredentialsIssuerCredentialsService.GetLastEidDataCredentialIssuerUrl("ndlseven");

                // get the last one
                var offerUrl = await _CountyPermitResidenceCredentialsIssuerCredentialsService.GetLastEidDataCredentialIssuerUrl();

                CountyResidenceDataMessage = "Add your County Residence data credentials to your wallet";
                CredentialOfferUrl = offerUrl;
                HasCountyResidenceData = true;
            }
            else
            {
                CountyResidenceDataMessage = "You have no valid County Residence data";
            }
        }
    }
}
