using Posterr.CoreObjects.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.AppInterfaces
{
    public interface IPostApplication
    {
        Task<Post> GetPostByIdAsync(Guid id);

        Task<IEnumerable<Post>> GetPostsByUserAsync(Guid userId, int offset, int fetch);

        Task<IEnumerable<Post>> GetLatestPostsAsync(Guid userId, bool justFollowing, int offset, int fetch);

        Task AddPostAsync(Post newPost);
    }
}
