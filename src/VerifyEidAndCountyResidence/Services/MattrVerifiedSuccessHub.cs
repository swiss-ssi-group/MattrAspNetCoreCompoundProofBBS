using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace VerifyEidAndCountyResidence.Services
{
    public class MattrVerifiedSuccessHub : Hub
    {
        /// <summary>
        /// This should be replaced with a cache which expires or something
        /// </summary>
        public static readonly ConcurrentDictionary<string, string> Challenges = new ConcurrentDictionary<string, string>();

        public void AddChallenge(string base64ChallengeId, string connnectionId)
        {
            Challenges.TryAdd(base64ChallengeId, connnectionId);
        }

    }
}
