using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Passions
{
    public class SetInUser
    {
        public class Command : IRequest
        {
            public string UserId;
            
            public List<Passion> Passions;
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
                var ids = request.Passions.ConvertAll(passion => passion.Id);
                var passions = await _context.Passions
                    .Where(passion => ids.Contains(passion.Id))
                    .ToListAsync();

                var currentUser = await _context.Users
                    .Include(user => user.Passions)
                    .Where(user => user.Id == request.UserId)
                    .FirstAsync();

                currentUser.Passions = passions;

                _context.Users.Update(currentUser);

                await _context.SaveChangesAsync();
                
                return Unit.Value;
            }
        }
    }
}