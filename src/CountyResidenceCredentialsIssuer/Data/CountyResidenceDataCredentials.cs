using System;
using System.ComponentModel.DataAnnotations;

namespace CountyResidenceCredentialsIssuer.Data
{
    public class CountyResidenceDataCredentials
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid OidcIssuerId { get; set; }
        public string OidcIssuer { get; set; }
        public string Did { get; set; }
    }
}
