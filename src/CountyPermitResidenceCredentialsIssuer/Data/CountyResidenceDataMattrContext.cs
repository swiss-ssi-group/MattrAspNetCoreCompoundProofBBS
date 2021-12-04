using Microsoft.EntityFrameworkCore;

namespace CountyPermitResidenceCredentialsIssuer.Data
{
    public class CountyResidenceDataMattrContext : DbContext
    {
        public CountyResidenceDataMattrContext(DbContextOptions<CountyResidenceDataMattrContext> options) : base(options)
        { }

        public DbSet<CountyResidenceDataCredentials> EidDataCredentials { get; set; }
    }
}
