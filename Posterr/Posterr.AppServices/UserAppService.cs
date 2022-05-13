using Posterr.Base;
using Posterr.Base.Exceptions;
using Posterr.CoreObjects.AppInterfaces;
using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posterr.AppServices
{
    public class UserAppService : IUserApplication
    {
        protected IServiceProvider ServiceProvider { get; }
        protected IUnitOfWorkFactory UowFactory { get; private set; }

        public UserAppService(IServiceProvider serviceProvider, IUnitOfWorkFactory workFactory)
        {
            ServiceProvider = serviceProvider;
            UowFactory = workFactory;
        }

        public async Task FollowUserAsync(Guid userId, Guid userToFollowId)
        {
            if (userId == userToFollowId) throw new UserCannotFollowSelfException();
            using (var uow = UowFactory.Create())
            {
                var userData = await uow.UsersRepository.GetByKeyAsync(userId);
                if (userData == null) throw new UserNotFoundException();

                var userToFollowData = await uow.UsersRepository.GetByKeyAsync(userToFollowId);
                if (userToFollowData == null) throw new UserNotFoundException();

                var userFollowingsData = await uow.UsersRepository.GetUserFollowingsAsync(userId, userToFollowId);
                if (userFollowingsData != null) return;

                DateTime referenceDate = TimeProvider.Current.UtcNow;

                var userStatsData = await uow.UsersRepository.GetUserStatsAsync(userId);
                userStatsData.NumberOfFollowing++;
                userStatsData.LastUpdate = referenceDate;

                var userToFollowStatsData = await uow.UsersRepository.GetUserStatsAsync(userToFollowId);
                userToFollowStatsData.NumberOfFollowers++;
                userToFollowStatsData.LastUpdate = referenceDate;

                var following = new UserFollowings()
                {
                    UserId = userId,
                    FollowsThisId = userToFollowId,
                    Since = referenceDate,
                };
                await uow.UsersRepository.InsertAsync(following);
                uow.UsersRepository.Update(userStatsData);
                uow.UsersRepository.Update(userToFollowStatsData);
                await uow.CommitAsync();
            }
        }

        public async Task UnfollowUserAsync(Guid userId, Guid userToFollowId)
        {
            if (userId == userToFollowId) throw new UserCannotFollowSelfException();
            using (var uow = UowFactory.Create())
            {
                var userData = await uow.UsersRepository.GetByKeyAsync(userId);
                if (userData == null) throw new UserNotFoundException();

                var userToFollowData = await uow.UsersRepository.GetByKeyAsync(userToFollowId);
                if (userToFollowData == null) throw new UserNotFoundException();

                var userFollowingsData = await uow.UsersRepository.GetUserFollowingsAsync(userId, userToFollowId);
                if (userFollowingsData == null) return;

                DateTime referenceDate = TimeProvider.Current.UtcNow;

                var userStatsData = await uow.UsersRepository.GetUserStatsAsync(userId);
                userStatsData.NumberOfFollowing--;
                userStatsData.LastUpdate = referenceDate;

                var userToFollowStatsData = await uow.UsersRepository.GetUserStatsAsync(userToFollowId);
                userToFollowStatsData.NumberOfFollowers--;
                userToFollowStatsData.LastUpdate = referenceDate;

                uow.UsersRepository.Delete(userFollowingsData);
                uow.UsersRepository.Update(userStatsData);
                uow.UsersRepository.Update(userToFollowStatsData);
                await uow.CommitAsync();
            }
        }

        public async Task<User> GetUserById(Guid id)
        {
            using (var uow = UowFactory.Create())
            {
                var userData = await uow.UsersRepository.GetByKeyAsync(id);
                if (userData == null) throw new UserNotFoundException();
                return userData;
            }
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            using (var uow = UowFactory.Create())
            {
                var user = await uow.UsersRepository.GetUserByUserNameAsync(userName);
                if (user == null) throw new UserNotFoundException();
                return await Task.FromResult(user);
            }
        }

        public async Task<UserStats> GetUserStatsAsync(Guid id)
        {
            using (var uow = UowFactory.Create())
            {
                var userData = await uow.UsersRepository.GetUserStatsAsync(id);
                if (userData == null) throw new UserNotFoundException();
                return userData;
            }
        }

        public async Task<bool> CanPostAsync(IUnitOfWork uow, Guid userId)
        {
            var userData = await uow.UsersRepository.GetByKeyAsync(userId);
            if (userData == null) throw new UserNotFoundException();
            var postsStats = await uow.UsersRepository.GetUserPostsStatsAsync(userId, TimeProvider.Current.UtcNow.Date);
            if (postsStats == null) return true;
            if (postsStats.PostsCounter < 5) return true;
            return false;
        }

        public async Task PostAddedAsync(IUnitOfWork uow, Guid userId)
        {

            var userData = await uow.UsersRepository.GetByKeyAsync(userId);
            if (userData == null) throw new UserNotFoundException();

            var referenceDate = TimeProvider.Current.UtcNow;

            var postsStats = await uow.UsersRepository.GetUserPostsStatsAsync(userId, referenceDate.Date);
            if (postsStats == null)
            {
                var userPostsStats = new UserPostStats()
                {
                    UserId = userId,
                    ReferenceDate = referenceDate.Date,
                    PostsCounter = 1,
                };
                await uow.UsersRepository.InsertAsync(userPostsStats);
            }
            else
            {
                postsStats.PostsCounter++;
                uow.UsersRepository.Update(postsStats);
            }

            var userStats = await uow.UsersRepository.GetUserStatsAsync(userId);
            userStats.NumberOfPosts++;
            uow.UsersRepository.Update(userStats);
        }

        public async Task<IEnumerable<UserFollowings>> GetUserFollowingsAsync(IUnitOfWork uow, Guid userId)
        {
            return await uow.UsersRepository.GetUserFollowingsAsync(userId);
        }

        public async Task<UserFollowings> GetUserFollowingEntryAsync(Guid userId, Guid followedUserId)
        {
            using (var uow = UowFactory.Create())
            {
                var entry = await uow.UsersRepository.GetUserFollowingsAsync(userId, followedUserId);
                return entry;
            }
        }
    }
}
