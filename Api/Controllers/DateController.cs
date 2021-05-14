using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.Dates;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class DateController : BaseApiController
    {
        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpGet("by-user")]
        public async Task<ActionResult<List<DateResponse>>> GetDatesByUser([FromQuery] int skip, [FromQuery] int limit)
        {
            var currentUser = HttpContext.GetUser();
            var dates = await Mediator.Send(new ReadManyByUser.Query
            {
                UserId = HttpContext.GetUserId(),
                Skip = skip,
                Limit = limit
            });

            dates.ForEach(date => date.Swap(currentUser));
            return Mapper.Map<List<Date>, List<DateResponse>>(dates);
        }

        [Authorize]
        [RequireAccess(Access.Common)]
        [HttpPost("chat")]
        public async Task<IActionResult> ChatWithDate(ChatMessageRequest request)
        {
            var message = Mapper.Map<ChatMessageRequest, ChatMessage>(request);
            await Mediator.Send(new ChatWithDate.Command
            {
                ChatMessage = message,
                UserId = HttpContext.GetUserId()
            });

            return Ok();
        }
    }
}