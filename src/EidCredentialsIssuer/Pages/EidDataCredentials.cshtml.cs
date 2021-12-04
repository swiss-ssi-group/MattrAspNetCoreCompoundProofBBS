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
            var medicinalProductCodeClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/medicinal_product_code");
            var numberOfDosesClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/number_of_doses");
            var totalNumberOfDosesClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/total_number_of_doses");
            var vaccinationDateClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/vaccination_date");
            var countryOfVaccinationClaim = User.Claims.FirstOrDefault(t => t.Type == $"https://{_mattrConfiguration.TenantSubdomain}/country_of_vaccination");

            if (familyNameClaim == null
                || givenNameClaim == null
                || dateOfBirthClaim == null
                || medicinalProductCodeClaim == null
                || numberOfDosesClaim == null
                || totalNumberOfDosesClaim == null
                || vaccinationDateClaim == null
                || countryOfVaccinationClaim == null)
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
                    MedicinalProductCode = medicinalProductCodeClaim.Value,
                    NumberOfDoses = numberOfDosesClaim.Value,
                    TotalNumberOfDoses = totalNumberOfDosesClaim.Value,
                    VaccinationDate = vaccinationDateClaim.Value,
                    CountryOfVaccination = countryOfVaccinationClaim.Value
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
