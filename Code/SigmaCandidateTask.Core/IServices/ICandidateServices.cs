using SigmaCandidateTask.Core.ViewModels.Candidate;

namespace SigmaCandidateTask.Core.IServices
{
    public interface ICandidateServices
    {
        Task  AddOrUpdateAsync(CandidateViewModel model);

    }
}
