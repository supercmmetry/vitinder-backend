using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Matches
{
    public class ReadManyByOther
    {
        public class Query : IRequest<List<Match>>
        {
            public Guid OtherId { get; set; }
            
            public int Skip { get; set; }
            
            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Match>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Match>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Matches
                    .Where(match => match.OtherId == request.OtherId)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();
            }
        }
    }
}