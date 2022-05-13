using Posterr.CoreObjects.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.RepoInterfaces
{
    public interface IPostsRepository : IGenericRepository<Post, Guid>
    {
        Task<IEnumerable<Post>> GetLatestPostsAsync(int offset, int fetch);
        Task<IEnumerable<Post>> GetLatestPostsAsync(List<Guid> users, int offset, int fetch);
    }
}
