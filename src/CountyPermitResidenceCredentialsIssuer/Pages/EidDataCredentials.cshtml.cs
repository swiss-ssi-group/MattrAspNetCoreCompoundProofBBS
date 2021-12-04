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

        public string EidDataMessage { get; set; } = "Loading credentials";
        public bool HasEidData { get; set; } = false;
        public CountyResidenceData EidData { get; set; }
        public string CredentialOfferUrl { get; set; }
        public EidDataCredentialsModel(
            CountyPermitResidenceCredentialsIssuerCredentialsService CountyPermitResidenceCredentialsIssuerCredentialsService,
            IOptions<MattrConfiguration> mattrConfiguration)
        {
            _CountyPermitResidenceCredentialsIssuerCredentialsService = CountyPermitResidenceCredentialsIssuerCredentialsService;
            _mattrConfiguration = mattrConfiguration.Value;
        }
        public async Task OnGetAsync()
        {

            var identityHasEidDataClaims = true;

            var familyNameClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/family_name");
            var givenNameClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/given_name");
            var dateOfBirthClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/date_of_birth");
            var birthPlace = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/birth_place");
            var heightClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/height");
            var nationalityClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/nationality");
            var genderClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/gender");
         
            if (familyNameClaim == null
                || givenNameClaim == null
                || dateOfBirthClaim == null
                || birthPlace == null
                || heightClaim == null
                || nationalityClaim == null
                || genderClaim == null)
            {
                identityHasEidDataClaims = false;
            }

            if (identityHasEidDataClaims)
            {
                EidData = new CountyResidenceData
                {
                    FamilyName = familyNameClaim.Value,
                    GivenName = givenNameClaim.Value,
                    DateOfBirth = dateOfBirthClaim.Value,
                    BirthPlace = birthPlace.Value,
                    Height = heightClaim.Value,
                    Nationality = nationalityClaim.Value,
                    Gender = genderClaim.Value
                };
                // get per name
                //var offerUrl = await _CountyPermitResidenceCredentialsIssuerCredentialsService.GetLastEidDataCredentialIssuerUrl("ndlseven");

                // get the last one
                var offerUrl = await _CountyPermitResidenceCredentialsIssuerCredentialsService.GetLastEidDataCredentialIssuerUrl();

                EidDataMessage = "Add your E-ID data credentials to your wallet";
                CredentialOfferUrl = offerUrl;
                HasEidData = true;
            }
            else
            {
                EidDataMessage = "You have no valid E-ID data";
            }
        }
    }
}
