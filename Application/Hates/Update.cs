using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Hates
{
    public class Update
    {
        public class Command : IRequest
        {
            public Hate Hate { get; set; }
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
                var hate = await _context.Hates.FindAsync(request.Hate.Id);
                _mapper.Map(request.Hate, hate);
                _context.Hates.Update(hate);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}