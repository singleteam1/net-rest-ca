namespace Shared.Abstractions.Common
{
    public interface IRepository<TEntity, TIdType> where TEntity : class
    {
        public abstract IQueryable<TEntity> QueryableSet { get; }
        public abstract Task<TEntity?> GetAsync(TIdType _id);
        public abstract Task<TEntity> CreateAsync(TEntity entity);
        public abstract Task<TEntity> UpdateAsync(TEntity entity);
        public abstract Task<bool> DeleteAsync(TEntity entity);
    }
}
