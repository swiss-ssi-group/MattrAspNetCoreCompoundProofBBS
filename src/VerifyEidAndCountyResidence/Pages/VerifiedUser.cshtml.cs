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
                    // Common
                    DateOfBirth = verifiedDataUser.DateOfBirth,
                    FamilyName = verifiedDataUser.FamilyName,
                    GivenName = verifiedDataUser.GivenName,

                    //// E-ID
                    //BirthPlace = verifiedDataUser.BirthPlace,
                    //Height = verifiedDataUser.Height,
                    //Nationality = verifiedDataUser.Nationality,
                    //Gender = verifiedDataUser.Gender,

                    //// County Residence
                    //AddressCountry = verifiedDataUser.AddressCountry,
                    //AddressLocality = verifiedDataUser.AddressLocality,
                    //AddressRegion = verifiedDataUser.AddressRegion,
                    //StreetAddress = verifiedDataUser.StreetAddress,
                    //PostalCode = verifiedDataUser.PostalCode
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
