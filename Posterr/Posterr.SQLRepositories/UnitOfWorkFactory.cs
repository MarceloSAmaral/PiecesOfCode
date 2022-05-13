using Posterr.CoreObjects.RepoInterfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Posterr.SQLRepositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
        public IUnitOfWork Create()
        {
            var context = ServiceProvider.GetService<PosterrDataContext>();
            return new UnitOfWork(context);
        }
    }
}
