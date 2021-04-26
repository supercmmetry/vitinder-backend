using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Matches
{
    public class ReadOne
    {
        public class Query : IRequest<Match>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Match>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Match> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Matches.FindAsync(request.Id);
            }
        }
    }
}