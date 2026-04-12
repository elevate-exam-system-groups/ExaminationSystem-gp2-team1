using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class StudentDiplomaEnrollmentConfiguration : IEntityTypeConfiguration<StudentDiplomaEnrollment>
    {
        public void Configure(EntityTypeBuilder<StudentDiplomaEnrollment> builder)
        {
            builder.ToTable("StudentDiplomaEnrollments");

            builder.HasKey(sde => sde.Id);

            // Composite unique constraint (one enrollment per student per diploma)
            builder.HasIndex(sde => new { sde.StudentId, sde.DiplomaId })
                .IsUnique()
                .HasDatabaseName("IX_StudentDiplomaEnrollments_StudentId_DiplomaId_Unique");

            // Indexes
            builder.HasIndex(sde => sde.StudentId)
                .HasDatabaseName("IX_StudentDiplomaEnrollments_StudentId");

            builder.HasIndex(sde => sde.DiplomaId)
                .HasDatabaseName("IX_StudentDiplomaEnrollments_DiplomaId");
        }
    }
}