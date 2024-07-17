using Microsoft.AspNetCore.Mvc;
using SigmaCandidateTask.Core.IServices;
using SigmaCandidateTask.Core.ViewModels.Candidate;

namespace SigmaCandidateTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateServices _candidateService;

        public CandidatesController(ICandidateServices candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateViewModel candidate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _candidateService.AddOrUpdateAsync(candidate);
            return Ok("Candidate added or updated successfully.");
        }

    }
}
