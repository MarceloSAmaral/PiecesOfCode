using Posterr.CoreObjects.Entities;
using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posterr.SQLRepositories
{
    public class PostsRepository : GenericRepository<Post, Guid>, IPostsRepository
    {
        public PostsRepository(PosterrDataContext context) : base(context)
        {
            Context = context;
        }

        public PosterrDataContext Context { get; }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(int offset, int fetch)
        {
            var records = Context.Posts.OrderByDescending(x => x.PostedAt).Skip(offset).Take(fetch).ToList();
            return await Task.FromResult(records);
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(List<Guid> usersIds, int offset, int fetch)
        {
            var records = Context.Posts.Where(x=> usersIds.Contains(x.UserId)).OrderByDescending(x => x.PostedAt).Skip(offset).Take(fetch).ToList();
            return await Task.FromResult(records);
        }
    }
}
