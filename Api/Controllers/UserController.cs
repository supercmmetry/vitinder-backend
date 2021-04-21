using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Extensions;
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
        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] int skip, [FromQuery] int limit)
        {
            return await Mediator.Send(new ReadMany.Query {Skip = skip, Limit = limit});
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser([FromQuery] string id)
        {
            return await Mediator.Send(new ReadOne.Query {Id = id});
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            return Ok(await Mediator.Send(new Create.Command {User = user}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpPatch]
        public async Task<IActionResult> UpdateUser(User user)
        {
            return Ok(await Mediator.Send(new Update.Command {User = user}));
        }

        [Authorize]
        [RequireAccess(Access.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet("me")]
        public ActionResult<UserResponse> GetCurrentUser()
        {
            return Mapper.Map<User, UserResponse>(HttpContext.GetCurrentUser());
        }

        [Authorize]
        [RequireAccess(Access.Anonymous)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserRequest userRequest)
        {
            var user = Mapper.Map<UserRequest, User>(userRequest);
            
            user.Id = HttpContext.GetFirebaseUserId();
            user.AccessLevel = (int) Access.Common;
            
            await Mediator.Send(new Create.Command {User = user});
            return Ok();
        }
    }
}