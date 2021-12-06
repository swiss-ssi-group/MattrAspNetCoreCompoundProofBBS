using Microsoft.EntityFrameworkCore;

namespace VerifyEidAndCountyResidence.Data
{
    public class VerifyEidCountyResidenceMattrContext : DbContext
    {
        public VerifyEidCountyResidenceMattrContext(DbContextOptions<VerifyEidCountyResidenceMattrContext> options) : base(options)
        { }

        public DbSet<EidCountyResidenceDataPresentationTemplate> EidAndCountyResidenceDataPresentationTemplates { get; set; }

        public DbSet<EidCountyResidenceDataPresentationVerify> EidAndCountyResidenceDataPresentationVerifications { get; set; }

        public DbSet<VerifiedEidAndCountyResidenceData> VerifiedEidAndCountyResidenceData { get; set; }

        public DbSet<Did> Dids { get; set; }


    }
}
