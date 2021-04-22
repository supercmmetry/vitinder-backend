using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Hates
{
    public class SetInUser
    {
        public class Command : IRequest
        {
            public string UserId;
            
            public List<Hate> Hates;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            
            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var ids = request.Hates.ConvertAll(hate => hate.Id);
                var hates = await _context.Hates
                    .Where(hate => ids.Contains(hate.Id))
                    .ToListAsync();

                var currentUser = await _context.Users
                    .Include(user => user.Hates)
                    .Where(user => user.Id == request.UserId)
                    .FirstAsync();

                currentUser.Hates = hates;

                _context.Users.Update(currentUser);

                await _context.SaveChangesAsync();
                
                return Unit.Value;
            }
        }
    }
}