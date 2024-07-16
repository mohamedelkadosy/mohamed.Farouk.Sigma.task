using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Interfaces;

namespace Framework.Core.IRepositories
{

    public interface IBaseRepositoryAsync<TEntity, TPrimeryKey> : IAsyncDisposable
       where TEntity : class, IEntityIdentity<TPrimeryKey>
    {
    
    }
}
