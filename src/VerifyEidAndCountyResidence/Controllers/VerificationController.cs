using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
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
        ///  {
        ///	 "presentationType": "QueryByFrame",
        ///	 "challengeId": "nGu/E6eQ8AraHzWyB/kluudUhraB8GybC3PNHyZI",
        ///	 "claims": {
        ///		"id": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb",
        ///		"http://schema.org/birth_place": "Seattle",
        ///		"http://schema.org/date_of_birth": "1953-07-21",
        ///		"http://schema.org/family_name": "Bob",
        ///		"http://schema.org/gender": "Male",
        ///		"http://schema.org/given_name": "Lammy",
        ///		"http://schema.org/height": "176cm",
        ///		"http://schema.org/nationality": "USA",
        ///		"http://schema.org/address_country": "Schweiz",
        ///		"http://schema.org/address_locality": "Thun",
        ///		"http://schema.org/address_region": "Bern",
        ///		"http://schema.org/postal_code": "3000",
        ///		"http://schema.org/street_address": "Thunerstrasse 14"
        ///	 },
        ///	 "verified": true,
        ///	 "holder": "did:key:z6MkmGHPWdKjLqiTydLHvRRdHPNDdUDKDudjiF87RNFjM2fb"
        ///  }
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> VerificationDataCallback()
        {
            string content = await new System.IO.StreamReader(Request.Body).ReadToEndAsync();
            var body = JsonSerializer.Deserialize<VerifiedEidCountyResidenceData>(content);

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