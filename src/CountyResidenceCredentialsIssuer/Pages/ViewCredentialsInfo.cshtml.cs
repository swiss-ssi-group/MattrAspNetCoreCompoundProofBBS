using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CountyResidenceCredentialsIssuer.Pages
{
    public class ViewLastCredentialsInfoModel : PageModel
    {
        private readonly CountyResidenceCredentialsIssuerCredentialsService _countyResidenceCredentialsIssuerCredentialsService;

        public string LatestCountyResidenceDid { get; set; }
        public string LatestCountyResidenceDataCallback { get; set; }

        public string CredentialOfferUrl { get; set; }
        public ViewLastCredentialsInfoModel(CountyResidenceCredentialsIssuerCredentialsService countyResidenceCredentialsIssuerCredentialsService)
        {
            _countyResidenceCredentialsIssuerCredentialsService = countyResidenceCredentialsIssuerCredentialsService;
        }
        public async Task OnGetAsync()
        {
            var credentialIssuer = await _countyResidenceCredentialsIssuerCredentialsService.GetLastCountyResidenceCredentialIssuer();
            LatestCountyResidenceDataCallback = credentialIssuer.Callback;
            LatestCountyResidenceDid = credentialIssuer.DidId;
        }
    }
}
