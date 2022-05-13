using System;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.RepoInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();

        IPostsRepository PostsRepository { get; }

        IUsersRepository UsersRepository { get; }
    }
}
