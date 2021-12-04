using Microsoft.EntityFrameworkCore;

namespace CountyPermitResidenceCredentialsIssuer.Data
{
    public class EidDataMattrContext : DbContext
    {
        public EidDataMattrContext(DbContextOptions<EidDataMattrContext> options) : base(options)
        { }

        public DbSet<EidDataCredentials> EidDataCredentials { get; set; }
    }
}
