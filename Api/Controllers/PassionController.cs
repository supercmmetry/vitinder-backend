using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.Passions;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class PassionController : BaseApiController
    {
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet("all")]
        public async Task<ActionResult<List<PassionResponse>>> GetPassions([FromQuery] int skip, [FromQuery] int limit)
        {
            var passions = await Mediator.Send(new ReadMany.Query {Skip = skip, Limit = limit});
            return Mapper.Map<List<Passion>, List<PassionResponse>>(passions);
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet]
        public async Task<ActionResult<PassionResponse>> GetPassion([FromQuery] Guid id)
        {
            var passion = await Mediator.Send(new ReadOne.Query {Id = id});
            return Mapper.Map<Passion, PassionResponse>(passion);
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePassion(PassionCreate passion)
        {
            return Ok(await Mediator.Send(new Create.Command {Passion = Mapper.Map<PassionCreate, Passion>(passion)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPatch]
        public async Task<IActionResult> UpdatePassion(PassionUpdate passion)
        {
            return Ok(await Mediator.Send(new Update.Command {Passion = Mapper.Map<PassionUpdate, Passion>(passion)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeletePassion([FromQuery] Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpPost("set")]
        public async Task<IActionResult> SetPassions(List<PassionRequest> passions)
        {
            return Ok(await Mediator.Send(new SetInUser.Command
            {
                UserId = HttpContext.GetUserId(),
                Passions = Mapper.Map<List<PassionRequest>, List<Passion>>(passions)
            }));
        }
    }
}