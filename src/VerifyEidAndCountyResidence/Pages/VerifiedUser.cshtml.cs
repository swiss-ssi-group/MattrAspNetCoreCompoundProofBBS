using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;
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

        public string Base64ChallengeId { get; set; }
        public EidCountyResidenceVerifiedClaimsDto VerifiedEidCountyResidenceDataClaims { get; private set; }

        public async Task OnGetAsync(string base64ChallengeId)
        {
            // user query param to get challenge id and display data
            if (base64ChallengeId != null)
            {
                var valueBytes = Convert.FromBase64String(base64ChallengeId);
                var challengeId = Encoding.UTF8.GetString(valueBytes);

                var verifiedDataUser = await _verifyEidCountyResidenceDbService.GetVerifiedUser(challengeId);
                VerifiedEidCountyResidenceDataClaims = new EidCountyResidenceVerifiedClaimsDto
                {
                    // Common
                    DateOfBirth = verifiedDataUser.DateOfBirth,
                    FamilyName = verifiedDataUser.FamilyName,
                    GivenName = verifiedDataUser.GivenName,

                    // E-ID
                    BirthPlace = verifiedDataUser.BirthPlace,
                    Height = verifiedDataUser.Height,
                    Nationality = verifiedDataUser.Nationality,
                    Gender = verifiedDataUser.Gender,

                    // County Residence
                    AddressCountry = verifiedDataUser.AddressCountry,
                    AddressLocality = verifiedDataUser.AddressLocality,
                    AddressRegion = verifiedDataUser.AddressRegion,
                    StreetAddress = verifiedDataUser.StreetAddress,
                    PostalCode = verifiedDataUser.PostalCode
                };
            }
        }
    }

    public class EidCountyResidenceVerifiedClaimsDto
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string Height { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }

        public string AddressCountry { get; set; }
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
    }
}
