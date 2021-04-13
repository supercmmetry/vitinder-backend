using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Middlewares;
using Application.Users;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UserController : BaseApiController
    {
        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] int skip, [FromQuery] int limit)
        {
            return await Mediator.Send(new ReadMany.Query{Skip = skip, Limit = limit});
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            return await Mediator.Send(new ReadOne.Query{Id = id});
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            return Ok(await Mediator.Send(new Create.Command {User = user}));
        }
        
        [HttpPatch]
        public async Task<IActionResult> UpdateUser(User user)
        {
            return Ok(await Mediator.Send(new Update.Command {User = user}));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
    }
}