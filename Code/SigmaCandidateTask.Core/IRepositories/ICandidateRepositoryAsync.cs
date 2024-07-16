using Framework.Core.IRepositories;
using SigmaCandidateTask.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaCandidateTask.Core.IRepositories
{
    public interface ICandidateRepositoryAsync : IBaseRepositoryAsync<Candidate, long>
    {

    }
}
