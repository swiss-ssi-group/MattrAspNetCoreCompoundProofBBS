using Microsoft.EntityFrameworkCore;

namespace EidCredentialsIssuer.Data
{
    public class VaccinationDataMattrContext : DbContext
    {
        public VaccinationDataMattrContext(DbContextOptions<VaccinationDataMattrContext> options) : base(options)
        { }

        public DbSet<VaccinationDataCredentials> VaccinationDataCredentials { get; set; }
    }
}
