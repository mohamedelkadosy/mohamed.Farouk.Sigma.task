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

        public CandidateServices(ICandidateRepositoryAsync candidateRepository, IUnitOfWorkAsync unitOfWorkAsync , IMapper mapper)
        {
            _candidateRepository = candidateRepository;
            _unitOfWorkAsync = unitOfWorkAsync; 
            _mapper = mapper;
        }

        public async Task ValidateModelAsync(CandidateViewModel model)
        {
            await Task.Run(() =>
            {
                var existEntity = this._candidateRepository.GetAsync(null).Result.FirstOrDefault(entity =>
                                entity.Email == model.Email  && entity.Id != model.Id);
                if (existEntity != null)
                    throw new Exception("This Email is Already exist");
            });
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
                var existingCandidate = await this._candidateRepository.FirstOrDefaultAsync(x => x.Id == model.Id, null);
                _mapper.Map(model, existingCandidate);
                candidate = await _candidateRepository.UpdateAsync(existingCandidate);
            }

            await _unitOfWorkAsync.CommitAsync();

        }
    }
}
