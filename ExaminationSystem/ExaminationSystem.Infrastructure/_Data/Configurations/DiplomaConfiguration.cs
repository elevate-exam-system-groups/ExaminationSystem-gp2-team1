using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.ToTable("Diplomas");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .HasMaxLength(1000);

            // Navigation properties
            builder.HasMany(d => d.Quizzes)
                .WithOne(q => q.Diploma)
                .HasForeignKey(q => q.DiplomaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Enrollments)
                .WithOne(de => de.Diploma)
                .HasForeignKey(de => de.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}