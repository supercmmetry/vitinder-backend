using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Dates
{
    public class ReadManyByUser
    {
        public class Query : IRequest<List<Date>>
        {
            public string UserId { get; set; }

            public int Skip { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Date>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Date>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Dates
                    .Include(date => date.User)
                    .ThenInclude(user => user.Passions)
                    .Include(date => date.User)
                    .ThenInclude(user => user.Hates)
                    .Include(date => date.User)
                    .ThenInclude(user => user.ProfileImage)
                    .Include(date => date.Other)
                    .ThenInclude(user => user.Passions)
                    .Include(date => date.Other)
                    .ThenInclude(user => user.Hates)
                    .Include(date => date.Other)
                    .ThenInclude(user => user.ProfileImage)
                    .AsSplitQuery()
                    .Where(date => date.UserId == request.UserId || date.OtherId == request.UserId)
                    .OrderByDescending(date => date.Timestamp)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();
            }
        }
    }
}