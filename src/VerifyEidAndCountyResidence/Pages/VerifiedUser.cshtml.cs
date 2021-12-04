using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace VerifyEidAndCountyResidence.Pages
{
    public class VerifiedUserModel : PageModel
    {
        private readonly VerifyEidCountyResidenceDbService _verifyEidCountyResidenceDbService;

        public VerifiedUserModel(VerifyEidCountyResidenceDbService verifyEidCountyResidenceDbService)
        {
            _verifyEidCountyResidenceDbService = verifyEidCountyResidenceDbService;
        }

        public string ChallengeId { get; set; }
        public EidCountyResidenceVerifiedClaimsDto VerifiedEidCountyResidenceDataClaims { get; private set; }

        public async Task OnGetAsync(string challengeId)
        {
            // user query param to get challenge id and display data
            if (challengeId != null)
            {
                var verifiedDataUser = await _verifyEidCountyResidenceDbService.GetVerifiedUser(challengeId);
                VerifiedEidCountyResidenceDataClaims = new EidCountyResidenceVerifiedClaimsDto
                {
                    DateOfBirth = verifiedDataUser.DateOfBirth,
                    MedicinalProductCode = verifiedDataUser.MedicinalProductCode,
                    FamilyName = verifiedDataUser.FamilyName,
                    GivenName = verifiedDataUser.GivenName,
                    VaccinationDate = verifiedDataUser.VaccinationDate,
                    CountryOfVaccination = verifiedDataUser.CountryOfVaccination
                };
            }
        }
    }

    public class EidCountyResidenceVerifiedClaimsDto
    {
        public string MedicinalProductCode { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DateOfBirth { get; set; }
        public string VaccinationDate { get; set; }
        public string CountryOfVaccination { get; set; }
    }
}
