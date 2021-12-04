using System;
using System.ComponentModel.DataAnnotations;

namespace CountyPermitResidenceCredentialsIssuer.Data
{
    public class EidDataCredentials
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid OidcIssuerId { get; set; }
        public string OidcIssuer { get; set; }
        public string Did { get; set; }
    }
}
