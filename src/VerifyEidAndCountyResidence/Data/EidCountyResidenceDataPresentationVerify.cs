using System;
using System.ComponentModel.DataAnnotations;

namespace VerifyEidAndCountyResidence.Data
{
    public class EidCountyResidenceDataPresentationVerify
    {
        [Key]
        public int Id { get; set; }
        public string DidEid { get; set; }
        public string DidCountyResidence { get; set; }
        public string TemplateId { get; set; }
        public string CallbackUrl { get; set; }
        public string InvokePresentationResponse { get; set; }
        public string Did { get; set; }
        public string SignAndEncodePresentationRequestBody { get; set; }
        public string Challenge { get; set; }
    }
}
