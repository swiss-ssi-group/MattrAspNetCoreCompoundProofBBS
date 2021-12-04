using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace VerifyEidAndCountyResidence.Pages
{
    public class VerifiedUserModel : PageModel
    {
        private readonly VerifyEidCountyResidenceDbService _VerifyEidAndCountyResidenceDbService;

        public VerifiedUserModel(VerifyEidCountyResidenceDbService VerifyEidAndCountyResidenceDbService)
        {
            _VerifyEidAndCountyResidenceDbService = VerifyEidAndCountyResidenceDbService;
        }

        public string ChallengeId { get; set; }
        public VaccineVerifiedClaimsDto VerifiedVaccinationDataClaims { get; private set; }

        public async Task OnGetAsync(string challengeId)
        {
            // user query param to get challenge id and display data
            if (challengeId != null)
            {
                var verifiedVaccinationDataUser = await _VerifyEidAndCountyResidenceDbService.GetVerifiedUser(challengeId);
                VerifiedVaccinationDataClaims = new VaccineVerifiedClaimsDto
                {
                    DateOfBirth = verifiedVaccinationDataUser.DateOfBirth,
                    MedicinalProductCode = verifiedVaccinationDataUser.MedicinalProductCode,
                    FamilyName = verifiedVaccinationDataUser.FamilyName,
                    GivenName = verifiedVaccinationDataUser.GivenName,
                    VaccinationDate = verifiedVaccinationDataUser.VaccinationDate,
                    CountryOfVaccination = verifiedVaccinationDataUser.CountryOfVaccination
                };
            }
        }
    }

    public class VaccineVerifiedClaimsDto
    {
        public string MedicinalProductCode { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DateOfBirth { get; set; }
        public string VaccinationDate { get; set; }
        public string CountryOfVaccination { get; set; }
    }
}
