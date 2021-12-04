using Microsoft.EntityFrameworkCore;

namespace VerifyEidAndCountyResidence.Data
{
    public class VerifyEidAndCountyResidenceMattrContext : DbContext
    {
        public VerifyEidAndCountyResidenceMattrContext(DbContextOptions<VerifyEidAndCountyResidenceMattrContext> options) : base(options)
        { }

        public DbSet<EidAndCountyResidenceDataPresentationTemplate> EidAndCountyResidenceDataPresentationTemplates { get; set; }

        public DbSet<EidAndCountyResidenceDataPresentationVerify> EidAndCountyResidenceDataPresentationVerifications { get; set; }

        public DbSet<VerifiedEidAndCountyResidenceData> VerifiedEidAndCountyResidenceData { get; set; }

        public DbSet<Did> Dids { get; set; }


    }
}
