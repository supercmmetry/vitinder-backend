using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Middlewares;
using Application.Dates;
using Domain;
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
            var currentUser = HttpContext.GetCurrentUser();
            var dates = await Mediator.Send(new ReadManyByUser.Query
            {
                UserId = HttpContext.GetFirebaseUserId(),
                Skip = skip,
                Limit = limit
            });

            dates.ForEach(date => date.Swap(currentUser));
            return Mapper.Map<List<Date>, List<DateResponse>>(dates);
        }
    }
}