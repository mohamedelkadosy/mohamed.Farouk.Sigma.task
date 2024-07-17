using System.ComponentModel.DataAnnotations;

namespace SigmaCandidateTask.Core.ViewModels.Candidate
{
    public class CandidateViewModel
    {
        public long? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PreferredCallTime { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? GitHubProfileUrl { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
