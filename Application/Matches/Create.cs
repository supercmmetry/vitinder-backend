using System;
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

namespace Application.Matches
{
    public class Create
    {
        public class Command : IRequest
        {
            public Match Match { get; set; }
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
                var alreadyExists = _context.Matches.Any(obj =>
                    obj.UserId == request.Match.UserId && obj.OtherId == request.Match.OtherId);

                if (alreadyExists)
                {
                    throw new DetailedException(
                        HttpStatusCode.Conflict,
                        ErrorMetadata.ResourceAlreadyExists,
                        "Match already exists"
                    );
                }

                _context.Matches.Add(request.Match);

                var sendFcm = false;
                
                if (request.Match.Status)
                {
                    var inverseMatchExists = await _context.Matches
                        .AnyAsync(
                            match => match.UserId == request.Match.OtherId &&
                                     match.OtherId == request.Match.UserId &&
                                     match.Status
                        );

                    if (inverseMatchExists)
                    {
                        _context.Dates.Add(new Date
                        {
                            UserId = request.Match.UserId,
                            OtherId = request.Match.OtherId,
                            Timestamp = DateTime.Now
                        });

                        sendFcm = true;
                    }
                }

                await _context.SaveChangesAsync();

                if (sendFcm)
                {
                    var date = await _context.Dates
                        .Where(obj => obj.UserId == request.Match.UserId && obj.OtherId == request.Match.OtherId)
                        .FirstAsync();
                    
                    // FCM
                    var user = await _context.Users
                        .Include(obj => obj.ProfileImage)
                        .Where(obj => obj.Id == request.Match.UserId)
                        .FirstAsync();
                    
                    var other = await _context.Users
                        .Include(obj => obj.ProfileImage)
                        .Where(obj => obj.Id == request.Match.OtherId)
                        .FirstAsync();

                    if (user.FcmToken != null)
                    {
                        var message = new Message
                        {
                            Token = user.FcmToken,
                            Notification = new Notification
                            {
                                Title = "You got a Date!",
                                Body = "You got a date with " + other.FirstName,
                                ImageUrl = other.ProfileImage?.Url
                            },
                            Data = new Dictionary<string, string>()
                            {
                                {"DateId", date.Id.ToString()}
                            }
                        };

                        await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    }

                    if (other.FcmToken != null)
                    {
                        var message = new Message
                        {
                            Token = other.FcmToken,
                            Notification = new Notification
                            {
                                Title = "You got a Date!",
                                Body = "You got a date with " + user.FirstName,
                                ImageUrl = user.ProfileImage?.Url
                            },
                            Data = new Dictionary<string, string>()
                            {
                                {"DateId", date.Id.ToString()}
                            }
                        };

                        await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    }
                }

                return Unit.Value;
            }
        }
    }
}