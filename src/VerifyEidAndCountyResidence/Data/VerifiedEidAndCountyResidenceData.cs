using System.ComponentModel.DataAnnotations;

namespace VerifyEidAndCountyResidence.Data
{
    public class VerifiedEidAndCountyResidenceData
    {
        [Key]
        public string ChallengeId { get; set; }
        public string PresentationType { get; set; }
        public string ClaimsId { get; set; }
        public bool Verified { get; set; }
        public string Holder { get; set; }

        // Section Common
        public string DateOfBirth { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }

        // section E-ID
        public string BirthPlace { get; set; }
        public string Height { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }

        // section Country Residence
        public string AddressCountry { get; set; }
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
    }
}
