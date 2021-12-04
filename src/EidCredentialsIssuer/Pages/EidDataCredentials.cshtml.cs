using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using EidCredentialsIssuer.Data;
using EidCredentialsIssuer.Services;

namespace EidCredentialsIssuer.Pages
{
    public class EidDataCredentialsModel : PageModel
    {
        private readonly EidCredentialsIssuerCredentialsService _eidCredentialsIssuerCredentialsService;
        private readonly MattrConfiguration _mattrConfiguration;

        public string EidDataMessage { get; set; } = "Loading credentials";
        public bool HasEidData { get; set; } = false;
        public EidData EidData { get; set; }
        public string CredentialOfferUrl { get; set; }
        public EidDataCredentialsModel(
            EidCredentialsIssuerCredentialsService eidCredentialsIssuerCredentialsService,
            IOptions<MattrConfiguration> mattrConfiguration)
        {
            _eidCredentialsIssuerCredentialsService = eidCredentialsIssuerCredentialsService;
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
                EidData = new EidData
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
                //var offerUrl = await _eidCredentialsIssuerCredentialsService.GetLastVaccinationDataCredentialIssuerUrl("ndlseven");

                // get the last one
                var offerUrl = await _eidCredentialsIssuerCredentialsService.GetLastEidDataCredentialIssuerUrl();

                EidDataMessage = "Add your E-ID data credentials to your wallet";
                CredentialOfferUrl = offerUrl;
                HasEidData = true;
            }
            else
            {
                EidDataMessage = "You have no valid vaccination data";
            }
        }
    }
}
