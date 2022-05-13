using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posterr.SQLRepositories
{
    public class UsersRepository : GenericRepository<User, Guid>, IUsersRepository
    {
        public UsersRepository(PosterrDataContext context) : base(context)
        {
            Context = context;
        }

        public PosterrDataContext Context { get; }

        public void Delete(UserFollowings userFollowingsData)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await Task.FromResult(Context.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault());
        }

        public async Task<UserFollowings> GetUserFollowingsAsync(Guid userId, Guid followedUserId)
        {
            return await Context.Followings.FindAsync(userId, followedUserId);
        }

        public async Task<IEnumerable<UserFollowings>> GetUserFollowingsAsync(Guid userId)
        {
            var records = Context.Followings.Where(x => x.UserId.Equals(userId));
            return await Task.FromResult(records);
        }

        public async Task<UserPostStats> GetUserPostsStatsAsync(Guid userId, DateTime referenceDate)
        {
            return await Task.FromResult(Context.UsersPostStats.Where(x => x.UserId.Equals(userId) && x.ReferenceDate.Equals(referenceDate)).FirstOrDefault());
        }

        public async Task<UserStats> GetUserStatsAsync(Guid userId)
        {
            return await Context.UsersStats.FindAsync(userId);
        }

        public async Task InsertAsync(UserFollowings following)
        {
            await Context.Followings.AddAsync(following);
        }

        public async Task InsertAsync(UserPostStats userPostsStats)
        {
            await Context.UsersPostStats.AddAsync(userPostsStats);
        }

        public void Update(UserStats userStatsData)
        {
            Context.UsersStats.Update(userStatsData);
        }

        public void Update(UserPostStats userPostsStats)
        {
            Context.UsersPostStats.Update(userPostsStats);
        }
    }
}
