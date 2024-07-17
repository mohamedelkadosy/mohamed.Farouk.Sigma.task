using Framework.Core.Interfaces;
using System.Linq.Expressions;

namespace Framework.Core.IRepositories
{

    public interface IBaseRepositoryAsync<TEntity, TPrimeryKey> : IAsyncDisposable
       where TEntity : class, IEntityIdentity<TPrimeryKey>
    {
        Task<TEntity> AddAsync(TEntity entity);

 
        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicat);

        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null);
    }
}
