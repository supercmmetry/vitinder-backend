using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<List<Passion>>> GetPassions([FromQuery] int skip, [FromQuery] int limit)
        {
            return await Mediator.Send(new ReadMany.Query {Skip = skip, Limit = limit});
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet]
        public async Task<ActionResult<Passion>> GetPassion([FromQuery] Guid id)
        {
            return await Mediator.Send(new ReadOne.Query {Id = id});
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePassion(PassionRequest passion)
        {
            return Ok(await Mediator.Send(new Create.Command {Passion = Mapper.Map<PassionRequest, Passion>(passion)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPatch]
        public async Task<IActionResult> UpdatePassion(PassionRequest passion)
        {
            return Ok(await Mediator.Send(new Update.Command {Passion = Mapper.Map<PassionRequest, Passion>(passion)}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeletePassion([FromQuery] Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
    }
}