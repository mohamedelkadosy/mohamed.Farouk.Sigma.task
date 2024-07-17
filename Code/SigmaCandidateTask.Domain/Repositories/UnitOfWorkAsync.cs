using SigmaCandidateTask.Core.IRepositories;
using SigmaCandidateTask.DataAccess.Contexts;

namespace SigmaCandidateTask.DataAccess.Repositories
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        #region Data Members
        private ApplicationDbContext _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance from type 
        /// UnitOfWorkAsync.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWorkAsync(ApplicationDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region IUnitOfWork	
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            var result = await this._context.SaveChangesAsync();
            return result;
        }
        #endregion
    }

}
