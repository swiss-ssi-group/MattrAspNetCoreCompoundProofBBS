using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EidCredentialsIssuer.Pages
{
    public class ViewLastCredentialsInfoModel : PageModel
    {
        private readonly EidCredentialsIssuerCredentialsService _eidCredentialsIssuerCredentialsService;

        public string LatestEidDid { get; set; }
        public string LatestEidDataCallback { get; set; }

        public string CredentialOfferUrl { get; set; }
        public ViewLastCredentialsInfoModel(EidCredentialsIssuerCredentialsService eidCredentialsIssuerCredentialsService)
        {
            _eidCredentialsIssuerCredentialsService = eidCredentialsIssuerCredentialsService;
        }
        public async Task OnGetAsync()
        {
            var credentialIssuer = await _eidCredentialsIssuerCredentialsService.GetLastEidCredentialIssuer();
            LatestEidDataCallback = credentialIssuer.Callback;
            LatestEidDid = credentialIssuer.DidId;
        }
    }
}
