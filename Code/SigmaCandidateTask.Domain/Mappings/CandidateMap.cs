using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SigmaCandidateTask.Entity;

namespace SigmaCandidateTask.DataAccess.Mappings
{
    public class CandidateMap : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(150).IsRequired().IsUnicode(false).HasColumnType("varchar(150)");
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnType("varchar(15)")
                .HasAnnotation("RegularExpression", @"^\+?[1-9]\d{1,14}$");

            builder.Property(x => x.LinkedInProfileUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnType("varchar(250)");

            builder.Property(x => x.GitHubProfileUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnType("varchar(250)");

            builder.Property(x => x.Comment)
                .HasMaxLength(500)
                .IsRequired();

        }
    }
}
