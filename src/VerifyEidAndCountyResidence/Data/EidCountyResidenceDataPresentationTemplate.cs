using System.ComponentModel.DataAnnotations;

namespace VerifyEidAndCountyResidence.Data
{
    public class EidCountyResidenceDataPresentationTemplate
    {
        [Key]
        public int Id { get; set; }
        public string DidEid { get; set; }      
        public string DidCountyResidence { get; set; }
        public string TemplateId { get; set; }
        public string MattrPresentationTemplateReponse { get; set; }
    }
}
