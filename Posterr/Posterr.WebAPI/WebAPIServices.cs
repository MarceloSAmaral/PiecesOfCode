using Posterr.CoreObjects.AppInterfaces;
using Posterr.CoreObjects.Entities;
using Posterr.WebAPI.Models;
using System;
using System.Threading.Tasks;

namespace Posterr.WebAPI
{

    public interface IWebAPIServices
    {
        User GetCurrentUser();

        Task<PostView> ConvertToPostViewAsync(Post post);
        Task<UserView> ConvertToUserViewAsync(Guid currentUserId, User userRecord, UserStats userStats);
    }


    public class WebAPIServices : IWebAPIServices
    {
        public IUserApplication UserApplication { get; }
        public IPostApplication PostApplication { get; }

        public WebAPIServices(IUserApplication userApplication, IPostApplication postApplication)
        {
            UserApplication = userApplication;
            PostApplication = postApplication;
        }
        public User GetCurrentUser()
        {
            return new User()
            {
                Id = new System.Guid("00000000-0000-0000-0001-000000000001"),
                UserName = "Myself",

                JoinDate = new System.DateTime(2022, 01, 25, 8, 0, 0)
            };
        }

        public async Task<PostView> ConvertToPostViewAsync(Post post)
        {
            var postView = new PostView();
            postView.Id = post.Id;
            postView.Author = (await UserApplication.GetUserById(post.UserId)).UserName;
            postView.Content = post.Content;
            postView.PostType = post.PostType;
            postView.PostedAt = post.PostedAt;

            switch (postView.PostType)
            {
                case TypesOfPosts.Original:
                    break;
                case TypesOfPosts.Repost:
                    postView.Origin = await LoadOriginPostAsync(post.RepostFrom.Value);
                    break;
                case TypesOfPosts.Quote:
                    postView.Origin = await LoadOriginPostAsync(post.QuoteFrom.Value);
                    break;
                case TypesOfPosts.NotDefined:
                default:
                    throw new NotImplementedException($"This method {nameof(ConvertToPostViewAsync)} is not ready for this type of post {postView.PostType}");
            }

            return postView;
        }

        private async Task<SimplifiedPostView> LoadOriginPostAsync(Guid originId)
        {
            var originPost = await PostApplication.GetPostByIdAsync(originId);
            return await ConvertToSimplifiedPostViewAsync(originPost);
        }

        private async Task<SimplifiedPostView> ConvertToSimplifiedPostViewAsync(Post post)
        {
            var simplifiedPostView = new SimplifiedPostView();
            simplifiedPostView.Id = post.Id;
            simplifiedPostView.Author = (await UserApplication.GetUserById(post.UserId)).UserName;
            simplifiedPostView.Content = post.Content;
            simplifiedPostView.PostedAt = post.PostedAt;
            return simplifiedPostView;
        }

        public async Task<UserView> ConvertToUserViewAsync(Guid currentUserId, User userRecord, UserStats userStats)
        {

            var userFollowingRecord = await UserApplication.GetUserFollowingEntryAsync(currentUserId, userRecord.Id);

            var userView = new UserView()
            {
                Followers = userStats.NumberOfFollowers,
                FollowingThis = userFollowingRecord == null ? false : true,
                Follows = userStats.NumberOfFollowing,
                JoinDate = userRecord.JoinDate,
                NumberOfPosts = userStats.NumberOfPosts,
                UserName = userRecord.UserName
            };
            return userView;
        }
    }
}
