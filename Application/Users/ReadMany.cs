using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Users
{
    public class ReadMany
    {
        public class Query : IRequest<List<User>>
        {
            public int Skip { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Users
                    .OrderBy(user => user.Id)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();
            }
        }
    }
}