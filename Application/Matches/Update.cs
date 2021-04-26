using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Matches
{
    public class Update
    {
        public class Command : IRequest
        {
            public Match Match { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            private readonly DataContext _context;
            private readonly IMapper _mapper;
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var match = await _context.Matches.FindAsync(request.Match.Id);
                _mapper.Map(request.Match, match);
                _context.Matches.Update(match);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}