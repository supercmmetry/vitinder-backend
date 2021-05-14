using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Dates
{
    public class ChatWithDate
    {
        public class Command : IRequest
        {
            public string UserId { get; set; }
            public ChatMessage ChatMessage { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var date = await _context.Dates
                    .Include(obj => obj.User)
                    .ThenInclude(obj => obj.ProfileImage)
                    .Include(obj => obj.Other)
                    .ThenInclude(obj => obj.ProfileImage)
                    .Where(obj => obj.Id == request.ChatMessage.DateId)
                    .FirstAsync();

                if (date == null)
                {
                    throw new DetailedException(HttpStatusCode.NotFound, ErrorMetadata.MissingResource,
                        "The given date does not exist");
                }

                if (date.UserId != request.UserId && date.OtherId != request.UserId)
                {
                    throw new DetailedException(HttpStatusCode.Conflict, ErrorMetadata.InsufficientAuthorization,
                        "You are not authorized to chat with someone-else's date.");
                }

                date.Swap(request.UserId);

                var message = new Message
                {
                    Token = date.Other.FcmToken,
                    Notification = new Notification
                    {
                        Title = date.User.FirstName,
                        Body = request.ChatMessage.Message,
                        ImageUrl = date.User.ProfileImage?.Url
                    },
                    Data = new Dictionary<string, string>()
                    {
                        {"DateId", date.Id.ToString()},
                        {
                            "Timestamp", request.ChatMessage.Timestamp
                                .ToUniversalTime()
                                .ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                        },
                        {"Message", request.ChatMessage.Message}
                    }
                };

                await FirebaseMessaging.DefaultInstance.SendAsync(message);

                return Unit.Value;
            }
        }
    }
}