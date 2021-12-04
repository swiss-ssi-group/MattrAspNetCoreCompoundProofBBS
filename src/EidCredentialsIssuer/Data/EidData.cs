namespace EidCredentialsIssuer.Data
{
    /// Could follow some definition like this
    /// https://schema.org/Person
    public class EidData
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string Height { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
    }
}

