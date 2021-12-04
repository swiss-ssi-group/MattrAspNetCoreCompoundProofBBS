namespace CountyResidenceCredentialsIssuer.Data
{
    /// Could follow some definition like this
    /// https://schema.org/PostalAddress
    public class CountyResidenceData
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string DateOfBirth { get; set; }
        public string AddressCountry { get; set; }
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
    }
}

