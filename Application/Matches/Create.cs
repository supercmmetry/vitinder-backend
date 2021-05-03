using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
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
                _context.Matches.Add(request.Match);
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
                        // todo: FCM
                        _context.Dates.Add(new Date
                        {
                            UserId = request.Match.UserId,
                            OtherId = request.Match.OtherId,
                            Timestamp = DateTime.Now
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}