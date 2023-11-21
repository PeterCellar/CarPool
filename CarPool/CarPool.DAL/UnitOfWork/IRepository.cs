using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.DAL.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> Get();
        void Delete(Guid entityId);
        Task<TEntity> InsertOrUpdateAsync<TModel>(
            TModel model,
            IMapper mapper,
            CancellationToken cancellationToken = default) where TModel : class;
    }
}
