using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CountyPermitResidenceCredentialsIssuer.Pages
{
    public class AdminModel : PageModel
    {
        private readonly MattrCredentialsService _mattrCredentialsService;
        public bool CreatingEidData { get; set; } = true;
        public string Callback { get; set; }

        [BindProperty]
        public IssuerCredential IssuerCredential { get; set; }
        public AdminModel(MattrCredentialsService mattrCredentialsService)
        {
            _mattrCredentialsService = mattrCredentialsService;
        }
        public void OnGet()
        {
            IssuerCredential = new IssuerCredential();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Callback = await _mattrCredentialsService.CreateCredentialsAndCallback(IssuerCredential.CredentialName);
            CreatingEidData = false;
            return Page();
        }
    }

    public class IssuerCredential
    {
        [Required]
        public string CredentialName { get; set; }
    }
}
