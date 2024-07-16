using Framework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaCandidateTask.Entity
{
    public class Candidate : IEntityIdentity<long>, IDateTimeSignature
    {
        public long Id { get; set; }
        public DateTime? FirstModificationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? PreferredCallTime { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? GitHubProfileUrl { get; set; }
        public string Comment { get; set; }

    }
}
