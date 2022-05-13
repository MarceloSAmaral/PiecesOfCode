using Posterr.CoreObjects.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.RepoInterfaces
{
    public interface IUsersRepository: IGenericRepository<User,Guid>
    {
        Task<UserPostStats> GetUserPostsStatsAsync(Guid userId, DateTime referenceDate);
        Task<UserStats> GetUserStatsAsync(Guid userId);
        Task<UserFollowings> GetUserFollowingsAsync(Guid userId, Guid userFollowdId);
        Task<IEnumerable<UserFollowings>> GetUserFollowingsAsync(Guid userId);
        Task InsertAsync(UserFollowings following);
        void Update(UserStats userStatsData);
        Task InsertAsync(UserPostStats userPostsStats);
        void Update(UserPostStats userPostsStats);
        void Delete(UserFollowings userFollowingsData);
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
