using Moq;
using Posterr.Base.Exceptions;
using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Posterr.AppServices.UnitTests
{
    public class UserAppServiceTests
    {
        public UserAppServiceTests()
        {
            _serviceProviderFake = new Mock<IServiceProvider>();
            _unitOfWorkFactoryFake = new Mock<IUnitOfWorkFactory>();
            _unitOfWorkFake = new Mock<IUnitOfWork>();
            _userRepoFake = new Mock<IUsersRepository>();
        }

        public Mock<IServiceProvider> _serviceProviderFake { get; set; }
        public Mock<IUnitOfWorkFactory> _unitOfWorkFactoryFake { get; set; }
        public Mock<IUnitOfWork> _unitOfWorkFake { get; set; }
        public Mock<IUsersRepository> _userRepoFake { get; set; }

        private void BasicMocksSetup()
        {
            _unitOfWorkFactoryFake.Setup(x => x.Create()).Returns(_unitOfWorkFake.Object);
            _unitOfWorkFake.Setup(x => x.UsersRepository).Returns(_userRepoFake.Object);
        }

        [Fact(DisplayName ="User cannot follow self.")]
        public async Task UserCannotFollowSelf()
        {
            Guid currendUserId = Guid.NewGuid();
            User currentUserData = new User();
            BasicMocksSetup();
            _userRepoFake.Setup(x => x.GetByKeyAsync(currendUserId)).ReturnsAsync(currentUserData);

            UserAppService userAppService = new UserAppService(_serviceProviderFake.Object, _unitOfWorkFactoryFake.Object);

            await Assert.ThrowsAsync<UserCannotFollowSelfException>(() => userAppService.FollowUserAsync(currendUserId, currendUserId));
        }

        [Fact(DisplayName ="WHEN user tries to follow invalid user OCCURS system throws UserNotFoundException.")]
        public async Task When_UserTriesToFollowInvalidUser_THEN_SystemThrowsUserNotFoundException()
        {
            Guid currendUserId = Guid.NewGuid();
            User currentUserData = new User();
            Guid invalidUserId = Guid.NewGuid();
            BasicMocksSetup();
            _userRepoFake.Setup(x => x.GetByKeyAsync(currendUserId)).ReturnsAsync(currentUserData);

            UserAppService userAppService = new UserAppService(_serviceProviderFake.Object, _unitOfWorkFactoryFake.Object);

            await Assert.ThrowsAsync<UserNotFoundException>(() => userAppService.FollowUserAsync(currendUserId, invalidUserId));
        }

        [Fact(DisplayName = "WHEN user tries to follow an already following user OCCURS system just returns ok without changind data.")]
        public async Task When_UserTriesToFollowAnAlreadyFollowingUser_THEN_System_reports_ok()
        {
            Guid currendUserId = Guid.NewGuid();
            User currentUserData = new User();
            Guid alreadyFollowingUserId = Guid.NewGuid();
            User alreadyFollowingUserIdData = new User();
            UserFollowings userFollowingsRecord = new UserFollowings();
            BasicMocksSetup();
            _userRepoFake.Setup(x => x.GetByKeyAsync(currendUserId)).ReturnsAsync(currentUserData);
            _userRepoFake.Setup(x => x.GetByKeyAsync(alreadyFollowingUserId)).ReturnsAsync(alreadyFollowingUserIdData);
            _userRepoFake.Setup(x => x.GetUserFollowingsAsync(currendUserId, alreadyFollowingUserId)).ReturnsAsync(userFollowingsRecord);
            UserAppService userAppService = new UserAppService(_serviceProviderFake.Object, _unitOfWorkFactoryFake.Object);

            await userAppService.FollowUserAsync(currendUserId, alreadyFollowingUserId);

            foreach (var item in _unitOfWorkFake.Invocations)
            {
                if (item.Method.Name == nameof(IUnitOfWork.CommitAsync)) throw new System.Exception("There should be no changes to data.");
            }
        }
    }
}
