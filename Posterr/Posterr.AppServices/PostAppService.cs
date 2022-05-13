using Microsoft.Extensions.DependencyInjection;
using Posterr.Base.Exceptions;
using Posterr.CoreObjects.AppInterfaces;
using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.AppServices
{
    public class PostAppService : IPostApplication
    {
        protected IServiceProvider ServiceProvider { get; }
        protected IUnitOfWorkFactory UowFactory { get; private set; }

        public PostAppService(IServiceProvider serviceProvider, IUnitOfWorkFactory workFactory)
        {
            ServiceProvider = serviceProvider;
            UowFactory = workFactory;
        }

        public async Task AddPostAsync(Post newPost)
        {
            using (var uow = UowFactory.Create())
            {
                var alreadyExists = await uow.PostsRepository.GetByKeyAsync(newPost.Id);
                if (alreadyExists != null) return;

                await ValidatePostAsync(uow, newPost);
                if(newPost.PostType == TypesOfPosts.Repost)
                {
                    newPost.Content = (await uow.PostsRepository.GetByKeyAsync(newPost.RepostFrom.Value)).Content;
                }

                IUserApplication userApplication = ServiceProvider.GetService<IUserApplication>();
                var canPost = await userApplication.CanPostAsync(uow, newPost.UserId);
                if (canPost == false) throw new MaxDailyPostsReachedException();

                await uow.PostsRepository.InsertAsync(newPost);
                await userApplication.PostAddedAsync(uow, newPost.UserId);
                await uow.CommitAsync();
            }
        }

        private async Task ValidatePostAsync(IUnitOfWork uow, Post newPost)
        {
            if (!String.IsNullOrWhiteSpace(newPost.Content))
            {
                if (newPost.Content.Length > 777) throw new MaxPostSizeExceededException();
            }

            switch (newPost.PostType)
            {
                case TypesOfPosts.NotDefined:
                    throw new BadPostEntryException("Post entry type should not be 'NotDefined'");
                case TypesOfPosts.Original:
                    await ValidateOriginalPostAsync(uow, newPost);
                    break;
                case TypesOfPosts.Repost:
                    await ValidateRepostPostAsync(uow, newPost);
                    break;
                case TypesOfPosts.Quote:
                    await ValidateQuotePostAsync(uow, newPost);
                    break;
                default:
                    break;
            }
        }

        private async Task ValidateOriginalPostAsync(IUnitOfWork uow, Post newPost)
        {
            if (string.IsNullOrWhiteSpace(newPost.Content)) throw new BadPostEntryException("Original post should have content.");
            if (newPost.QuoteFrom.HasValue) throw new BadPostEntryException("Original post should not reference another post entry for quote.");
            if (newPost.RepostFrom.HasValue) throw new BadPostEntryException("Original post should not reference another post entry for repost.");
            await Task.CompletedTask;
        }

        private async Task ValidateRepostPostAsync(IUnitOfWork uow, Post newPost)
        {
            if (!string.IsNullOrWhiteSpace(newPost.Content)) throw new BadPostEntryException("Repost post should not have own content.");
            if (newPost.QuoteFrom.HasValue) throw new BadPostEntryException("Repost post should not reference another post entry for quote.");
            if (newPost.RepostFrom.HasValue == false) throw new BadPostEntryException("Repost post should reference another post entry.");

            var origin = await uow.PostsRepository.GetByKeyAsync(newPost.RepostFrom.Value);
            if (origin == null) throw new BadPostEntryException("Repost post refers to a non existing post entry.");

            await Task.CompletedTask;
        }

        private async Task ValidateQuotePostAsync(IUnitOfWork uow, Post newPost)
        {
            if (string.IsNullOrWhiteSpace(newPost.Content)) throw new BadPostEntryException("Quote post should have content.");
            if (newPost.QuoteFrom.HasValue == false) throw new BadPostEntryException("Quote post should reference another post entry.");
            if (newPost.RepostFrom.HasValue) throw new BadPostEntryException("Quote post should not reference another post entry for repost.");

            var origin = await uow.PostsRepository.GetByKeyAsync(newPost.QuoteFrom.Value);
            if (origin == null) throw new BadPostEntryException("Quote post refers to a non existing post entry.");

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(Guid userId, bool justFollowing, int offset, int fetch)
        {
            using (var uow = UowFactory.Create())
            {
                if (justFollowing) return await GetLatestPostsFromFollowingsAsync(uow, userId, offset, fetch);
                else return await GetLatestPostsFromAnybodyAsync(uow, offset, fetch);
            }
        }

        private async Task<IEnumerable<Post>> GetLatestPostsFromFollowingsAsync(IUnitOfWork uow, Guid userId, int offset, int fetch)
        {
            IUserApplication userApplication = ServiceProvider.GetService<IUserApplication>();
            var usersToFollow = await userApplication.GetUserFollowingsAsync(uow, userId);
            if (usersToFollow == null) return null;
            List<Guid> usersIds = new List<Guid>();
            foreach (var item in usersToFollow)
            {
                usersIds.Add(item.FollowsThisId);
            }
            return await uow.PostsRepository.GetLatestPostsAsync(usersIds, offset, fetch);
        }

        private Task<IEnumerable<Post>> GetLatestPostsFromAnybodyAsync(IUnitOfWork uow, int offset, int fetch)
        {
            return uow.PostsRepository.GetLatestPostsAsync(offset, fetch);
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            using (var uow = UowFactory.Create())
            {
                var post = await uow.PostsRepository.GetByKeyAsync(id);
                if (post == null) throw new PostNotFoundException();
                return post;
            }
        }

        public async Task<IEnumerable<Post>> GetPostsByUserAsync(Guid userId, int offset, int fetch)
        {
            using (var uow = UowFactory.Create())
            {
                List<Guid> users = new List<Guid>();
                users.Add(userId);
                var posts = (await uow.PostsRepository.GetLatestPostsAsync(users, offset, fetch )).ToList();
                return await Task.FromResult(posts);
            }
       }
    }
}
