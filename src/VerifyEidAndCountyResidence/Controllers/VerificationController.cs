using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using VerifyEidAndCountyResidence.Services;

namespace VerifyEidAndCountyResidence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly VerifyEidCountyResidenceDbService _verifyEidAndCountyResidenceDbService;

        private readonly IHubContext<MattrVerifiedSuccessHub> _hubContext;

        public VerificationController(VerifyEidCountyResidenceDbService verifyEidAndCountyResidenceDbService,
            IHubContext<MattrVerifiedSuccessHub> hubContext)
        {
            _hubContext = hubContext;
            _verifyEidAndCountyResidenceDbService = verifyEidAndCountyResidenceDbService;
        }

        /// <summary>
        ///{
        ///    "presentationType": "QueryByFrame",
        ///    "challengeId": "RhOtpTa8vNh1EId6sJ7AVD3prerMMDSkfWZrUPzt",
        ///    "claims": {
        ///        "id": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb",
        ///        "http://schema.org/country_of_vaccination": "CH",
        ///        "http://schema.org/date_of_birth": "1953-07-21",
        ///        "http://schema.org/family_name": "Bob",
        ///        "http://schema.org/given_name": "Lammy",
        ///        "http://schema.org/medicinal_product_code": "Pfizer/BioNTech Comirnaty EU/1/20/1528",
        ///        "http://schema.org/number_of_doses": "2",
        ///        "http://schema.org/total_number_of_doses": "2",
        ///        "http://schema.org/vaccination_date": "2021-05-12"
        ///    },
        ///    "verified": true,
        ///    "holder": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb"
        ///}
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> VerificationDataCallback([FromBody] VerifiedEidCountyResidenceData body)
        {
            string connectionId;
            var found = MattrVerifiedSuccessHub.Challenges
                .TryGetValue(body.ChallengeId, out connectionId);

            // test Signalr
            //await _hubContext.Clients.Client(connectionId).SendAsync("MattrCallbackSuccess", $"{body.ChallengeId}");
            //return Ok();

            var exists = await _verifyEidAndCountyResidenceDbService.ChallengeExists(body.ChallengeId);

            if (exists)
            {
                await _verifyEidAndCountyResidenceDbService.PersistVerification(body);

                if (found)
                {
                    //$"/VerifiedUser?challengeid={body.ChallengeId}"
                    await _hubContext.Clients
                        .Client(connectionId)
                        .SendAsync("MattrCallbackSuccess", $"{body.ChallengeId}");
                }

                return Ok();
            }

            return BadRequest("unknown verify request");
        }

        //[HttpPost]
        //    [Route("[action]")]
        //    public async Task<IActionResult> VerificationDataCallback([FromBody] object body)
        //    {
        //        var test = body.ToString();
        //        return Ok();

        //    }
        //}
    }
}