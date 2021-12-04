using Microsoft.EntityFrameworkCore;
using VerifyEidAndCountyResidence.Data;
using System.Linq;
using System.Threading.Tasks;
using VerifyEidAndCountyResidence.Controllers;

namespace VerifyEidAndCountyResidence
{
    public class VerifyEidCountyResidenceDbService
    {
        private readonly VerifyEidCountyResidenceMattrContext _verifyEidCountyResidenceMattrContext;

        public VerifyEidCountyResidenceDbService(VerifyEidCountyResidenceMattrContext verifyEidAndCountyResidenceMattrContext)
        {
            _verifyEidCountyResidenceMattrContext = verifyEidAndCountyResidenceMattrContext;
        }

        public async Task<(string DidEid, string DidCountyResidence, string TemplateId)> GetLastVaccinationDataPresentationTemplate()
        {
            var eidCountyResidenceTemplate = await _verifyEidCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationTemplates
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (eidCountyResidenceTemplate != null)
            {
                var templateId = eidCountyResidenceTemplate.TemplateId;

                return (eidCountyResidenceTemplate.DidEid, 
                    eidCountyResidenceTemplate.DidCountyResidence, 
                    eidCountyResidenceTemplate.TemplateId);
            }

            return (string.Empty, string.Empty, string.Empty);
        }

        public async Task CreateEidAndCountyResidenceDataTemplate(EidCountyResidenceDataPresentationTemplate template)
        {
            _verifyEidCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationTemplates.Add(template);

            await _verifyEidCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task<bool> ChallengeExists(string challengeId)
        {
            return await _verifyEidCountyResidenceMattrContext
                .EidAndCountyResidenceDataPresentationVerifications
                .AnyAsync(d => d.Challenge == challengeId);
        }

        public async Task CreateEidAndCountyResidenceDataPresentationVerify(EidCountyResidenceDataPresentationVerify verify)
        {
            _verifyEidCountyResidenceMattrContext.EidAndCountyResidenceDataPresentationVerifications.Add(verify);
            await _verifyEidCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task PersistVerification(VerifiedEidCountyResidenceData item)
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

            _verifyEidCountyResidenceMattrContext.VerifiedEidAndCountyResidenceData.Add(data);
            await _verifyEidCountyResidenceMattrContext.SaveChangesAsync();
        }

        public async Task<VerifiedEidAndCountyResidenceData> GetVerifiedUser(string challengeId)
        {
            return await _verifyEidCountyResidenceMattrContext
                .VerifiedEidAndCountyResidenceData
                .FirstOrDefaultAsync(v => v.ChallengeId == challengeId);
        }

        public async Task<Did> GetDid(string name)
        {
            return await _verifyEidCountyResidenceMattrContext
                .Dids
                .FirstOrDefaultAsync(v => v.Name == name);
        }

        public async Task<Did> CreateDid(Did did)
        {
            _verifyEidCountyResidenceMattrContext.Dids.Add(did);
            await _verifyEidCountyResidenceMattrContext.SaveChangesAsync();
            return did;
        }
    }
}
