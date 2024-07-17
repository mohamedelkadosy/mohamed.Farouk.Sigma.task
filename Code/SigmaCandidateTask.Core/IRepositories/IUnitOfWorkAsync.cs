namespace SigmaCandidateTask.Core.IRepositories
{
    public interface IUnitOfWorkAsync
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();
        #endregion
    }
}
