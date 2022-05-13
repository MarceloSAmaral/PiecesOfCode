using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posterr.SQLRepositories
{
    public abstract class GenericRepository<TEntity, TKey> where TEntity : class
    {
        protected PosterrDataContext context;
        protected DbSet<TEntity> dbSet;

        protected GenericRepository(PosterrDataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByKeyAsync(TKey pk)
        {
            return await this.dbSet.FindAsync(pk);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await this.dbSet.AddAsync(entity);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await this.dbSet.AddRangeAsync(entities);
        }

        public virtual async Task Delete(TKey pk)
        {
            TEntity entityToDelete = await this.dbSet.FindAsync(pk);
            this.Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.dbSet.Attach(entityToDelete);
            }
            this.dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.dbSet.Attach(entityToUpdate);
            this.context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
        {
            this.dbSet.AttachRange(entitiesToUpdate);
            this.context.Entry(entitiesToUpdate).State = EntityState.Modified;
        }

    }
}
