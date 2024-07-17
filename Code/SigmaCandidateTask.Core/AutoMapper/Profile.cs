using AutoMapper;
using SigmaCandidateTask.Core.ViewModels.Candidate;
using SigmaCandidateTask.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaCandidateTask.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Candidate, CandidateViewModel>().ReverseMap();

            // DTO to ViewModel
            CreateMap<CandidateViewModel, CandidateViewModel>().ReverseMap();
        }
    }
}
