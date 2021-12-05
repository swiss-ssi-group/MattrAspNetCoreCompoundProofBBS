using System.Text.Json.Serialization;

namespace VerifyEidAndCountyResidence.Controllers
{
    /// <summary>
    /// This class totally depends on the OIDC credential issuer claims
    ///    "claims": {
    ///        "id": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb",
    ///        "http://schema.org/country_of_vaccination": "CH",
    ///        "http://schema.org/date_of_birth": "1953-07-21",
    ///        "http://schema.org/family_name": "Bob",
    ///        "http://schema.org/given_name": "Lammy",
    ///        "http://schema.org/medicinal_product_code": "Pfizer/BioNTech Comirnaty EU/1/20/1528",
    ///        "http://schema.org/number_of_doses": "2",
    ///        "http://schema.org/total_number_of_doses": "2",
    ///        "http://schema.org/vaccination_date": "2021-05-12"
    ///    },
    /// </summary>
    public class VerifiedEidCountyResidenceDataClaims
    {
        public string Id { get; set; }

        [JsonPropertyName("http://schema.org/date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("http://schema.org/family_name")]
        public string FamilyName { get; set; }

        [JsonPropertyName("http://schema.org/given_name")]
        public string GivenName { get; set; }

        // section E-ID
        [JsonPropertyName("http://schema.org/birth_place")]
        public string BirthPlace { get; set; }

        [JsonPropertyName("http://schema.org/height")]
        public string Height { get; set; }

        [JsonPropertyName("http://schema.org/nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("http://schema.org/gender")]
        public string Gender { get; set; }

        // section Country Residence
        [JsonPropertyName("http://schema.org/address_country")]
        public string AddressCountry { get; set; }

        [JsonPropertyName("http://schema.org/address_locality")]
        public string AddressLocality { get; set; }

        [JsonPropertyName("http://schema.org/address_region")]
        public string AddressRegion { get; set; }
        [JsonPropertyName("http://schema.org/street_address")]
        public string StreetAddress { get; set; }

        [JsonPropertyName("http://schema.org/postal_code")]
        public string PostalCode { get; set; }
    }
}
