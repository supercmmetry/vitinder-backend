using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.CloudinaryImages;
using Application.Users;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
        public ActionResult<UserResponse> GetMe()
        {
            return Mapper.Map<User, UserResponse>(HttpContext.GetUser());
        }
        
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMe(UserRequest userRequest)
        {
            var currentUser = HttpContext.GetUser();
            Mapper.Map(userRequest, currentUser);

            await Mediator.Send(new Update.Command
            {
                User = currentUser
            });
            
            return Ok();
        }

        [Authorize]
        [RequireAccess(Access.Anonymous)]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserRequest userRequest)
        {
            var user = Mapper.Map<UserRequest, User>(userRequest);
            
            user.Id = HttpContext.GetUserId();
            user.AccessLevel = (int) Access.Common;
            
            await Mediator.Send(new Create.Command {User = user});
            return Ok();
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet("recommend")]
        public async Task<ActionResult<List<UserResponse>>> Recommend([Range(1, 10)][FromQuery] int limit)
        {
            var recommendations = await Mediator.Send(new Recommend.Query
            {
                User = HttpContext.GetUser(),
                Limit = limit
            });

            return Mapper.Map<List<User>, List<UserResponse>>(recommendations);
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [ValidateFile("ProfilePicture", 10485760, "png", "jpg", "jpeg")]
        [HttpPost("profile-pic")]
        public async Task<IActionResult> AddProfilePic()
        {
            await Mediator.Send(new AddToUser.Command
            {
                User = HttpContext.GetUser(),
                File = HttpContext.GetValidatedFile("ProfilePicture"),
                Folder = Configuration["Cloudinary:Folders:ProfileImages"]
            });
            
            return Ok();
        }
    }
}