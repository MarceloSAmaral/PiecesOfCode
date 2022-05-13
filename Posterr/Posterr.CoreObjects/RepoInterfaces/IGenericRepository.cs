using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posterr.CoreObjects.RepoInterfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByKeyAsync(TKey pk);
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entitiesToUpdate);
    }
}
