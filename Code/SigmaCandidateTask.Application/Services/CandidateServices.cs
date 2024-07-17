using AutoMapper;
using SigmaCandidateTask.Core.IRepositories;
using SigmaCandidateTask.Core.IServices;
using SigmaCandidateTask.Core.ViewModels.Candidate;
using SigmaCandidateTask.Entity;

namespace SigmaCandidateTask.Application.Services
{
    public class CandidateServices : ICandidateServices
    {

        private readonly ICandidateRepositoryAsync _candidateRepository;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMapper _mapper;

        public CandidateServices(ICandidateRepositoryAsync candidateRepository, IUnitOfWorkAsync unitOfWorkAsync, IMapper mapper)
        {
            _candidateRepository = candidateRepository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _mapper = mapper;
        }

        public async Task ValidateModelAsync(CandidateViewModel model)
        {
            var existEntity = await this._candidateRepository.FirstOrDefaultAsync(entity =>
                            entity.Email == model.Email && entity.Id != model.Id);
            if (existEntity != null)
                throw new InvalidOperationException("This email is already in use.");
        }

        public async Task AddOrUpdateAsync(CandidateViewModel model)
        {
            await this.ValidateModelAsync(model);
            var candidate = new Candidate();

            if (model.Id == null)
            {
                candidate = _mapper.Map<Candidate>(model);
                candidate = await _candidateRepository.AddAsync(candidate);
            }
            else
            {
                var existingCandidate = await this._candidateRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (existingCandidate == null)
                    throw new InvalidOperationException("Candidate not found.");
                _mapper.Map(model, existingCandidate);
                candidate = await _candidateRepository.UpdateAsync(existingCandidate);
            }

            await _unitOfWorkAsync.CommitAsync();

        }
    }
}
