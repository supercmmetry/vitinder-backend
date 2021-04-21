using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Passions
{
    public class ReadMany
    {
        public class Query : IRequest<List<Passion>>
        {
            public int Skip { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Passion>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Passion>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Passions
                    .OrderBy(passion => passion.Id)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();
            }
        }
    }
}