using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Passions
{
    public class Update
    {
        public class Command : IRequest
        {
            public Passion Passion { get; set; }
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
                var passion = await _context.Passions.FindAsync(request.Passion.Id);
                _mapper.Map(request.Passion, passion);
                _context.Passions.Update(passion);
                
                return Unit.Value;
            }
        }
    }
}