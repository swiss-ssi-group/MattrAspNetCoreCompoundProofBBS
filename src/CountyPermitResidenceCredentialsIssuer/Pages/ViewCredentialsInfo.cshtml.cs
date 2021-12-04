using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CountyResidenceCredentialsIssuer.Pages
{
    public class ViewLastCredentialsInfoModel : PageModel
    {
        private readonly CountyPermitResidenceCredentialsIssuerCredentialsService _CountyPermitResidenceCredentialsIssuerCredentialsService;

        public string LatestEidDid { get; set; }
        public string LatestEidDataCallback { get; set; }

        public string CredentialOfferUrl { get; set; }
        public ViewLastCredentialsInfoModel(CountyPermitResidenceCredentialsIssuerCredentialsService CountyPermitResidenceCredentialsIssuerCredentialsService)
        {
            _CountyPermitResidenceCredentialsIssuerCredentialsService = CountyPermitResidenceCredentialsIssuerCredentialsService;
        }
        public async Task OnGetAsync()
        {
            var credentialIssuer = await _CountyPermitResidenceCredentialsIssuerCredentialsService.GetLastCountyResidenceCredentialIssuer();
            LatestEidDataCallback = credentialIssuer.Callback;
            LatestEidDid = credentialIssuer.DidId;
        }
    }
}
