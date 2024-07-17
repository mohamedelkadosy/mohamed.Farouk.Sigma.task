using Framework.Core.Interfaces;
using Framework.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.DataAccess.Repositories
{
    public class BaseRepositoryAsync<TEntity, TPrimaryKey> :
        IAsyncDisposable,
        IBaseRepositoryAsync<TEntity, TPrimaryKey>
        where TEntity : class, IEntityIdentity<TPrimaryKey>
    {
        #region Data Members
        private readonly DbContext _context;
        #endregion

        #region Constructors
        public BaseRepositoryAsync(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Entities = _context.Set<TEntity>();
        }
        #endregion

        #region Properties
        protected DbContext Context => _context;

        protected DbSet<TEntity> Entities { get; }
        #endregion

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            DateTime now = DateTime.UtcNow;
            if (entity is ICreationTimeSignature creationTimeSignature)
            {
                creationTimeSignature.CreationDate = now;
            }

            if (entity is IDateTimeSignature dateTimeSignature)
            {
                dateTimeSignature.FirstModificationDate = now;
                dateTimeSignature.LastModificationDate = now;
            }

            await Entities.AddAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity is IDateTimeSignature dateTimeSignature)
            {
                DateTime now = DateTime.UtcNow;
                if (dateTimeSignature.FirstModificationDate == null)
                {
                    dateTimeSignature.FirstModificationDate = now;
                    dateTimeSignature.LastModificationDate = now;
                }
                else
                {
                    dateTimeSignature.LastModificationDate = now;
                }
            }

            Entities.Update(entity);
            return await Task.FromResult(entity);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (Entities == null) throw new InvalidOperationException("Entities DbSet is not initialized.");

            IQueryable<TEntity> query = Entities;

            if (includedNavigationsList != null)
            {
                foreach (var navigation in includedNavigationsList)
                {
                    query = query.Include(navigation);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
        {
            if (Entities == null) throw new InvalidOperationException("Entities DbSet is not initialized.");

            IQueryable<TEntity> query = Entities;

            if (includedNavigationsList != null)
            {
                foreach (var navigation in includedNavigationsList)
                {
                    query = query.Include(navigation);
                }
            }

            return predicate == null ? await query.ToListAsync() : await query.Where(predicate).ToListAsync();
        }
    }
}
