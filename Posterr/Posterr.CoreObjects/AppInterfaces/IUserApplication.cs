using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.AppInterfaces
{

    public interface IUserApplication
    {
        Task<User> GetUserById(Guid id);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<UserStats> GetUserStatsAsync(Guid id);

        Task FollowUserAsync(Guid userId, Guid userToFollowId);

        Task UnfollowUserAsync(Guid userId, Guid userToFollowId);

        Task<bool> CanPostAsync(IUnitOfWork uof, Guid userId);

        Task PostAddedAsync(IUnitOfWork uow, Guid userId);

        Task<IEnumerable<UserFollowings>> GetUserFollowingsAsync(IUnitOfWork uow, Guid userId);

        Task<UserFollowings> GetUserFollowingEntryAsync(Guid userId, Guid followedUserId);
    }
}
