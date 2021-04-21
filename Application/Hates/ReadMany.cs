using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Hates
{
    public class ReadMany
    {
        public class Query : IRequest<List<Hate>>
        {
            public int Skip { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Hate>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Hate>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Hates
                    .OrderBy(hate => hate.Id)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();
            }
        }
    }
}