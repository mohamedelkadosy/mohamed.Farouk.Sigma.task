using Framework.DataAccess.Repositories;
using SigmaCandidateTask.Entity;
using SigmaCandidateTask.Core.IRepositories;
using SigmaCandidateTask.DataAccess.Contexts;

namespace SigmaCandidateTask.DataAccess.Repositories
{
    public class CandidateRepositoryAsync : BaseRepositoryAsync<Candidate, long>, ICandidateRepositoryAsync
    {
        public CandidateRepositoryAsync(ApplicationDbContext context): base(context)
        {

        }
    }
}
