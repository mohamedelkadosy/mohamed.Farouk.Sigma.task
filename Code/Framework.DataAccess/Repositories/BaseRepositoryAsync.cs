using Framework.Core.Interfaces;
using Framework.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Framework.DataAccess.Repositories
{
    public class BaseRepositoryAsync<TEntity, TPrimeryKey> :
        IAsyncDisposable,
        IBaseRepositoryAsync<TEntity, TPrimeryKey>
        where TEntity : class, IEntityIdentity<TPrimeryKey>

    {
        #region Data Members
        protected DbContext _context;
       
        #endregion

        #region Constructors
        public BaseRepositoryAsync(DbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Properties
        protected DbContext Context
        {
            get { return this._context; }
            set
            {
                this._context = value;
                this.Entities = this._context.Set<TEntity>();
            }
        }

        protected DbSet<TEntity> Entities { get; set; }
        #endregion


        public ValueTask DisposeAsync()
        {
            return this._context.DisposeAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            DateTime now = DateTime.Now;
            if (entity is ICreationTimeSignature)
            {
                var dateTimeSignature = (ICreationTimeSignature)entity;
                dateTimeSignature.CreationDate = now;
            }

            if (entity is IDateTimeSignature)
            {
                var dateTimeSignature = (IDateTimeSignature)entity;
                dateTimeSignature.FirstModificationDate = now;
                dateTimeSignature.LastModificationDate = now;
            }

            await this.Entities.AddAsync(entity);
            return entity;
        }


        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                if (entity is IDateTimeSignature)
                {
                    DateTime now = DateTime.Now;
                    var dateTimeSignature = (IDateTimeSignature)entity;

                    if (dateTimeSignature.FirstModificationDate.HasValue == false)
                    {
                        dateTimeSignature.FirstModificationDate = now;
                        dateTimeSignature.LastModificationDate = now;
                    }
                    else
                        dateTimeSignature.LastModificationDate = now;
                }


                this.Entities.Update(entity);
                return entity;
            });
        }




    }
}
