using Microsoft.EntityFrameworkCore;
using SigmaCandidateTask.DataAccess.Mappings;
using SigmaCandidateTask.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SigmaCandidateTask.DataAccess.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {}

        public ApplicationDbContext(){}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CandidateMap());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
        }

        public DbSet<Candidate> Candidates { get; set; }


    }
}
