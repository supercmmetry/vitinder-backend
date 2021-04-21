using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Hates
{
    public class ReadOne
    {
        public class Query : IRequest<Hate>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Hate>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Hate> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Hates.FindAsync(request.Id);
            }
        }
    }
}