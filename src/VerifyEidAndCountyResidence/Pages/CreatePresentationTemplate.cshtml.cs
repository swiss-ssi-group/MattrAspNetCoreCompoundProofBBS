using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VerifyEidAndCountyResidence.Pages
{
    public class CreatePresentationTemplateModel : PageModel
    {
        private readonly MattrPresentationTemplateService _mattrVerifyService;
        public bool CreatingPresentationTemplate { get; set; } = true;
        public string TemplateId { get; set; }

        [BindProperty]
        public PresentationTemplate PresentationTemplate { get; set; }
        public CreatePresentationTemplateModel(MattrPresentationTemplateService mattrVerifyService)
        {
            _mattrVerifyService = mattrVerifyService;
        }
        public void OnGet()
        {
            PresentationTemplate = new PresentationTemplate();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TemplateId = await _mattrVerifyService.CreatePresentationTemplateId(
                PresentationTemplate.DidEid, PresentationTemplate.DidCountyResidence);

            CreatingPresentationTemplate = false;
            return Page();
        }
    }

    public class PresentationTemplate
    {
        [Required]
        public string DidEid { get; set; }

        [Required]
        public string DidCountyResidence { get; set; }
        
    }
}
