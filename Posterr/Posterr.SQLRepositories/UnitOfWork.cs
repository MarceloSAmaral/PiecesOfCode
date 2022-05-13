using Posterr.CoreObjects.RepoInterfaces;
using System;
using System.Threading.Tasks;

namespace Posterr.SQLRepositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public PosterrDataContext _context;
        private PostsRepository _postsRepository;
        private UsersRepository _usersRepository;

        public UnitOfWork(PosterrDataContext context)
        {
            _context = context;
        }

        public UnitOfWork()
        {
            _context = new PosterrDataContext();
        }

        public IPostsRepository PostsRepository
        {
            get
            {
                return _postsRepository = _postsRepository ?? new PostsRepository(_context); 
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                return _usersRepository = _usersRepository ?? new UsersRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
