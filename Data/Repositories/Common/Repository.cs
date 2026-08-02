using Data.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Common;

namespace Data.Repositories.Common
{
    public class Repository<TEntity, TIdType> : IRepository<TEntity, TIdType> where TEntity : class
    {
        private readonly DataDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DataDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> QueryableSet => _dbSet.AsQueryable();

        public async Task<TEntity?> GetAsync(TIdType _id)
        {
            var result = await _dbSet.FindAsync(_id);
            return result;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var result = _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return result is not null;
        }
    }
}
