using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EidCredentialsIssuer.Pages
{
    public class ViewLastCredentialsInfoModel : PageModel
    {
        private readonly EidCredentialsIssuerCredentialsService _EidCredentialsIssuerCredentialsService;

        public string LatestVaccinationDid { get; set; }
        public string LatestVaccinationDataCallback { get; set; }

        public string CredentialOfferUrl { get; set; }
        public ViewLastCredentialsInfoModel(EidCredentialsIssuerCredentialsService EidCredentialsIssuerCredentialsService)
        {
            _EidCredentialsIssuerCredentialsService = EidCredentialsIssuerCredentialsService;
        }
        public async Task OnGetAsync()
        {
            var credentialIssuer = await _EidCredentialsIssuerCredentialsService.GetLastVaccineCredentialIssuer();
            LatestVaccinationDataCallback = credentialIssuer.Callback;
            LatestVaccinationDid = credentialIssuer.DidId;
        }
    }
}
