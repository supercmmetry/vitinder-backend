using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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

            private static Expression<Func<User, User, int>> ScoreBias()
            {
                return (user, other) => (user.FieldOfStudy == other.FieldOfStudy ? 2 : 0) +
                                        (user.YearOfStudy == other.YearOfStudy ? 1 : 0) +
                                        (user.FieldOfStudy == other.FieldOfStudy &&
                                         user.YearOfStudy == other.YearOfStudy
                                            ? 2
                                            : 0)
                                        - Math.Abs(user.Age - other.Age);
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = request.User;

                var matchingUserIds = (
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
                    .Select(user => user.Id)
                    .Take(request.Limit)
                    .ToListAsync();

                var potentialUserIds = (
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
                                    ).Count() * 2 +
                                    (
                                        from hate in _context.Hates
                                            .Include(hate => hate.Users)
                                        where hate.Users.Contains(currentUser) && hate.Users.Contains(user)
                                        select new { }
                                    ).Count() * 2 +
                                    ScoreBias().Invoke(currentUser, user)
                        }
                    )
                    .OrderByDescending(obj => obj.Score)
                    .Take(request.Limit)
                    .Select(obj => obj.User.Id)
                    .ToListAsync();

                var userIds = await Task.WhenAll(matchingUserIds, potentialUserIds);
                var recommendationIds = userIds[0];

                if (recommendationIds.Count < request.Limit)
                {
                    recommendationIds
                        .AddRange(userIds[1]
                            .GetRange(0, Math.Min(request.Limit - recommendationIds.Count, userIds[1].Count)));
                }

                var recommendations = await _context.Users
                    .Include(user => user.Passions)
                    .Include(user => user.Hates)
                    .Include(user => user.ProfileImage)
                    .AsSplitQuery()
                    .Where(user => recommendationIds.Contains(user.Id))
                    .ToListAsync();

                var recommendationMap = new Dictionary<string, User>();

                recommendations.ForEach(user => recommendationMap[user.Id] = user);
                
                return recommendationIds.Select(id => recommendationMap[id]).ToList();
            }
        }
    }
}