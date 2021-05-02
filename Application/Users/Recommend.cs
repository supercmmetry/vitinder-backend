using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Matches;
using Domain;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class Recommend
    {
        public class Query : IRequest<List<User>>
        {
            public User User { get; set; }

            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            private static Expression<Func<User, User, bool>> IsSexualMatch()
            {
                const string straight = "Straight";
                const string lesbian = "Lesbian";
                const string gay = "Gay";
                const string bisexual = "Bisexual";
                const string trans = "Transgender";
                const string queer = "Queer";
                const string male = "Male";
                const string female = "Female";

                return (me, other) =>
                    me.SexualOrientation == straight && other.SexualOrientation == straight && me.Sex == male &&
                        other.Sex == female ||
                        me.SexualOrientation == straight && other.SexualOrientation == straight && me.Sex == female &&
                        other.Sex == male ||
                        me.SexualOrientation == lesbian && other.SexualOrientation == lesbian && me.Sex == female &&
                        other.Sex == female ||
                        me.SexualOrientation == gay && other.SexualOrientation == gay && me.Sex == male &&
                        other.Sex == male ||
                        me.SexualOrientation == queer && other.SexualOrientation != straight ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == straight && me.Sex == male &&
                        other.Sex == female ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == straight && me.Sex == female &&
                        other.Sex == male ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == lesbian && me.Sex == female &&
                        other.Sex == female ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == gay && me.Sex == male &&
                        other.Sex == male ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == bisexual ||
                        me.SexualOrientation == bisexual && other.SexualOrientation == queer ||
                        me.SexualOrientation == trans && other.SexualOrientation == trans ||
                        me.SexualOrientation == trans && other.SexualOrientation == queer;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = request.User;

                var matchingUsers = (
                        from user in _context.Users.AsExpandable()
                        where user.Id != currentUser.Id
                        where !(
                            from match in _context.Matches
                            where match.UserId == currentUser.Id && match.OtherId == user.Id
                            select new { }
                        ).Any()
                        where IsSexualMatch().Invoke(currentUser, user)
                        join match in _context.Matches on user.Id equals match.UserId
                        where match.OtherId == currentUser.Id && match.Status
                        select user
                    )
                    .OrderBy(user => user.Id)
                    .Take(request.Limit)
                    .ToListAsync();

                var potentialUsers = (
                        from user in _context.Users.AsExpandable()
                        where user.Id != currentUser.Id
                        where !(
                            from match in _context.Matches
                            where match.UserId == currentUser.Id && match.OtherId == user.Id
                            select new { }
                        ).Any()
                        where IsSexualMatch().Invoke(currentUser, user)
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
                                ).Count() +
                                user.FieldOfStudy == currentUser.FieldOfStudy ? 2 :
                                0 +
                                user.YearOfStudy == currentUser.YearOfStudy ? 2 : 0
                                - Math.Abs(user.Age - currentUser.Age) / 2
                        }
                    )
                    .OrderByDescending(obj => obj.Score)
                    .Select(obj => obj.User)
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