using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.ToTable("QuizAttempts");

            builder.HasKey(qa => qa.Id);

            builder.Property(qa => qa.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("in_progress");

            builder.Property(qa => qa.Score)
                .HasPrecision(5, 2);

            builder.Property(qa => qa.TotalQuestions)
                .IsRequired();

    

            // Indexes
            builder.HasIndex(qa => qa.StudentId)
                .HasDatabaseName("IX_QuizAttempts_StudentId");

            builder.HasIndex(qa => qa.QuizId)
                .HasDatabaseName("IX_QuizAttempts_QuizId");

            builder.HasIndex(qa => qa.Status)
                .HasDatabaseName("IX_QuizAttempts_Status");

            builder.HasIndex(qa => new { qa.StudentId, qa.QuizId })
                .HasDatabaseName("IX_QuizAttempts_StudentId_QuizId");

            builder.HasIndex(qa => qa.StartedAt)
                .HasDatabaseName("IX_QuizAttempts_StartedAt");

            builder.HasIndex(qa => qa.DeadlineAt)
                .HasDatabaseName("IX_QuizAttempts_DeadlineAt");

            // Navigation properties
            builder.HasMany(qa => qa.Answers)
                .WithOne(aa => aa.Attempt)
                .HasForeignKey(aa => aa.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}