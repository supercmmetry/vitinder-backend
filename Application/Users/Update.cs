using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Users
{
    public class Update
    {
        public class Command : IRequest
        {
            public User User { get; set; }
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
                var user = await _context.Users.FindAsync(request.User.Id);
                _mapper.Map(request.User, user);
                _context.Users.Update(user);
                
                return Unit.Value;
            }
        }
    }
}