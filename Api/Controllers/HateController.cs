using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.Hates;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HateController : BaseApiController
    {
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet("all")]
        public async Task<ActionResult<List<HateResponse>>> GetHates([FromQuery] int skip, [FromQuery] int limit)
        {
            var hates = await Mediator.Send(new ReadMany.Query {Skip = skip, Limit = limit});
            return Mapper.Map<List<Hate>, List<HateResponse>>(hates);
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet]
        public async Task<ActionResult<HateResponse>> GetHate([FromQuery] Guid id)
        {
            var hate = await Mediator.Send(new ReadOne.Query {Id = id});
            return Mapper.Map<Hate, HateResponse>(hate);
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateHate(HateCreate hate)
        {
            return Ok(await Mediator.Send(new Create.Command {Hate = Mapper.Map<HateCreate, Hate>(hate)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPatch]
        public async Task<IActionResult> UpdateHate(HateUpdate hate)
        {
            return Ok(await Mediator.Send(new Update.Command {Hate = Mapper.Map<HateUpdate, Hate>(hate)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteHate([FromQuery] Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
        
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpPost("set")]
        public async Task<IActionResult> SetPassions(List<HateRequest> hates)
        {
            return Ok(await Mediator.Send(new SetInUser.Command
            {
                UserId = HttpContext.GetFirebaseUserId(),
                Hates = Mapper.Map<List<HateRequest>, List<Hate>>(hates)
            }));
        }
    }
}