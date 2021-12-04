using Microsoft.EntityFrameworkCore;
using VerifyEidAndCountyResidence.Data;
using System.Linq;
using System.Threading.Tasks;
using VerifyEidAndCountyResidence.Controllers;

namespace VerifyEidAndCountyResidence
{
    public class VerifyEidAndCountyResidenceDbService
    {
        private readonly VerifyEidAndCountyResidenceMattrContext _verifyEidAndCountyResidenceMattrContext;

        public VerifyEidAndCountyResidenceDbService(VerifyEidAndCountyResidenceMattrContext verifyEidAndCountyResidenceMattrContext)
        {
            _verifyEidAndCountyResidenceMattrContext = verifyEidAndCountyResidenceMattrContext;
        }

        public async Task<(string DidId, string TemplateId)> GetLastVaccinationDataPresentationTemplate()
        {
            var vaccineTemplate = await _verifyEidAndCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationTemplates
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (vaccineTemplate != null)
            {
                var templateId = vaccineTemplate.TemplateId;
                return (vaccineTemplate.DidId, vaccineTemplate.TemplateId);
            }

            return (string.Empty, string.Empty);
        }

        public async Task CreateEidAndCountyResidenceDataTemplate(EidAndCountyResidenceDataPresentationTemplate template)
        {
            _verifyEidAndCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationTemplates.Add(template);

            await _verifyEidAndCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task<bool> ChallengeExists(string challengeId)
        {
            return await _verifyEidAndCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationVerifications
                .AnyAsync(d => d.Challenge == challengeId);
        }

        public async Task CreateEidAndCountyResidenceDataPresentationVerify(EidAndCountyResidenceDataPresentationVerify verify)
        {
            _verifyEidAndCountyResidenceMattrContext.EidAndCountyResidenceDataPresentationVerifications.Add(verify);
            await _verifyEidAndCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task PersistVerification(VerifiedVaccinationData item)
        {
            var data = new VerifiedEidAndCountyResidenceData
            {
                ClaimsId = item.Claims.Id,
                //CountryOfVaccination = item.Claims.CountryOfVaccination,
                DateOfBirth = item.Claims.DateOfBirth,
                FamilyName = item.Claims.FamilyName,
                GivenName = item.Claims.GivenName,
                MedicinalProductCode = item.Claims.MedicinalProductCode,
                //NumberOfDoses = item.Claims.NumberOfDoses,
                //TotalNumberOfDoses = item.Claims.TotalNumberOfDoses,
                //VaccinationDate = item.Claims.VaccinationDate,

                ChallengeId = item.ChallengeId,
                Holder = item.Holder,
                PresentationType = item.PresentationType,
                Verified = item.Verified
            };

            _verifyEidAndCountyResidenceMattrContext.VerifiedEidAndCountyResidenceData.Add(data);
            await _verifyEidAndCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task<VerifiedEidAndCountyResidenceData> GetVerifiedUser(string challengeId)
        {
            return await _verifyEidAndCountyResidenceMattrContext
                .VerifiedEidAndCountyResidenceData
                .FirstOrDefaultAsync(v => v.ChallengeId == challengeId);
        }

        public async Task<Did> GetDid(string name)
        {
            return await _verifyEidAndCountyResidenceMattrContext
                .Dids
                .FirstOrDefaultAsync(v => v.Name == name);
        }

        public async Task<Did> CreateDid(Did did)
        {
            _verifyEidAndCountyResidenceMattrContext.Dids.Add(did);
            await _verifyEidAndCountyResidenceMattrContext.SaveChangesAsync();
            return did;
        }
    }
}
