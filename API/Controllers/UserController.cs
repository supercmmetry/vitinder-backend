using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] int skip, [FromQuery] int limit)
        {
            return await Mediator.Send(new ReadMany.Query{Skip = skip, Limit = limit});
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
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
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
    }
}