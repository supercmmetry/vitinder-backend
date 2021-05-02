using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.Matches;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MatchController : BaseApiController
    {
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpPost("by-user")]
        public async Task<IActionResult> CreateMatchByUser(MatchRequest matchRequest)
        {
            var match = Mapper.Map<MatchRequest, Match>(matchRequest);
            match.UserId = HttpContext.GetFirebaseUserId();

            await Mediator.Send(new Create.Command
            {
                Match = match
            });

            return Ok();
        }
    }
}