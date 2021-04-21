using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Passions
{
    public class ReadOne
    {
        public class Query : IRequest<Passion>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Passion>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Passion> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Passions.FindAsync(request.Id);
            }
        }
    }
}