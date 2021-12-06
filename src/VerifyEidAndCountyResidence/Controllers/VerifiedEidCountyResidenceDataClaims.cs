using System.Text.Json.Serialization;

namespace VerifyEidAndCountyResidence.Controllers
{
    /// <summary>
    ///  {
    ///	 "presentationType": "QueryByFrame",
    ///	 "challengeId": "nGu/E6eQ8AraHzWyB/kluudUhraB8GybC3PNHyZI",
    ///	 "claims": {
    ///		"id": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb",
    ///		"http://schema.org/birth_place": "Seattle",
    ///		"http://schema.org/date_of_birth": "1953-07-21",
    ///		"http://schema.org/family_name": "Bob",
    ///		"http://schema.org/gender": "Male",
    ///		"http://schema.org/given_name": "Lammy",
    ///		"http://schema.org/height": "176cm",
    ///		"http://schema.org/nationality": "USA",
    ///		"http://schema.org/address_country": "Schweiz",
    ///		"http://schema.org/address_locality": "Thun",
    ///		"http://schema.org/address_region": "Bern",
    ///		"http://schema.org/postal_code": "3000",
    ///		"http://schema.org/street_address": "Thunerstrasse 14"
    ///	 },
    ///	 "verified": true,
    ///	 "holder": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb"
    ///  }
    /// </summary>
    public class VerifiedEidCountyResidenceDataClaims
    {
        [JsonPropertyName("id")]
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
