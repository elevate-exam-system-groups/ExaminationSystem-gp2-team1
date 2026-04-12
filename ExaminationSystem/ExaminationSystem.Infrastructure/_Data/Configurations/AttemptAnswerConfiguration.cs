using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
        {
            builder.ToTable("AttemptAnswers");

            builder.HasKey(aa => aa.Id);

            builder.Property(aa => aa.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(aa => aa.AttemptId)
                .HasDatabaseName("IX_AttemptAnswers_AttemptId");

            builder.HasIndex(aa => aa.QuestionId)
                .HasDatabaseName("IX_AttemptAnswers_QuestionId");

            builder.HasIndex(aa => new { aa.AttemptId, aa.QuestionId })
                .IsUnique()
                .HasDatabaseName("IX_AttemptAnswers_AttemptId_QuestionId_Unique");

            // Foreign key to QuestionOption (nullable - student can skip)
            builder.HasOne(aa => aa.SelectedOption)
                .WithMany()
                .HasForeignKey(aa => aa.SelectedOptionId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}