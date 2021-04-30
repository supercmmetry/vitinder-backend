using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class Recommend
    {
        public class Query : IRequest<List<User>>
        {
            public string UserId { get; set; }

            public int Skip { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = await _context.Users
                    .Include(user => user.Passions)
                    .Include(user => user.Hates)
                    .Where(user => user.Id == request.UserId)
                    .FirstAsync();

                var matchingUsers = (
                        from user in _context.Users
                        where user.Id != currentUser.Id
                        where !(
                            from match in _context.Matches
                            where match.UserId == currentUser.Id && match.OtherId == user.Id
                            select new { }
                        ).Any()
                        where (currentUser.SexualOrientation == "Straight" && user.SexualOrientation == "Straight" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Gay" && user.SexualOrientation == "Gay" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Lesbian" && user.SexualOrientation == "Lesbian" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Bisexual" && (currentUser.Sex == "Male" &&
                                                                               user.SexualOrientation == "Gay") ||
                               (currentUser.Sex == "Female" &&
                                user.SexualOrientation == "Lesbian") || user.SexualOrientation == "Straight" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Transgender" &&
                               user.SexualOrientation == "Transgender")
                        join match in _context.Matches on user.Id equals match.UserId
                        where match.OtherId == currentUser.Id && match.Status
                        select user
                    )
                    .OrderBy(user => user.Id)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();

                var potentialUsers = (
                        from user in _context.Users
                        where user.Id != currentUser.Id
                        where !(
                            from match in _context.Matches
                            where match.UserId == currentUser.Id && match.OtherId == user.Id
                            select new { }
                        ).Any()
                        where (currentUser.SexualOrientation == "Straight" && user.SexualOrientation == "Straight" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Gay" && user.SexualOrientation == "Gay" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Lesbian" && user.SexualOrientation == "Lesbian" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Bisexual" && (currentUser.Sex == "Male" &&
                                                                               user.SexualOrientation == "Gay") ||
                               (currentUser.Sex == "Female" &&
                                user.SexualOrientation == "Lesbian") || user.SexualOrientation == "Straight" ||
                               user.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Queer") ||
                              (currentUser.SexualOrientation == "Transgender" &&
                               user.SexualOrientation == "Transgender")
                        join match in _context.Matches on user.Id equals match.UserId into matches
                        from match in matches.DefaultIfEmpty()
                        where match == null
                        select new
                        {
                            User = user,
                            Score = (
                                        from passion in _context.Passions
                                            .Include(passion => passion.Users)
                                        where passion.Users.Contains(currentUser) && passion.Users.Contains(user)
                                        select new { }
                                    ).Count() +
                                    (
                                        from hate in _context.Hates
                                            .Include(hate => hate.Users)
                                        where hate.Users.Contains(currentUser) && hate.Users.Contains(user)
                                        select new { }
                                    ).Count()
                        }
                    )
                    .OrderByDescending(obj => obj.Score)
                    .Select(obj => obj.User)
                    .Skip(request.Skip)
                    .Take(request.Limit)
                    .ToListAsync();

                var users = await Task.WhenAll(matchingUsers, potentialUsers);
                var recommendations = users[0];

                if (recommendations.Count < request.Limit)
                {
                    recommendations
                        .AddRange(users[1]
                            .GetRange(0, Math.Min(request.Limit - recommendations.Count, users[1].Count)));
                }

                return recommendations;
            }
        }
    }
}