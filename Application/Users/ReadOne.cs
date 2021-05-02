using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class ReadOne
    {
        public class Query : IRequest<User>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Users
                    .Where(user => user.Id == request.Id)
                    .Include(user => user.Passions)
                    .Include(user => user.Hates)
                    .Include(user => user.ProfileImage)
                    .AsSplitQuery()
                    .FirstAsync();
            }
        }
    }
}