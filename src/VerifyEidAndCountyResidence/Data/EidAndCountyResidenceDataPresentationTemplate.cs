using System.ComponentModel.DataAnnotations;

namespace VerifyEidAndCountyResidence.Data
{
    public class EidAndCountyResidenceDataPresentationTemplate
    {
        [Key]
        public int Id { get; set; }
        public string DidId { get; set; }
        public string TemplateId { get; set; }
        public string MattrPresentationTemplateReponse { get; set; }
    }
}
