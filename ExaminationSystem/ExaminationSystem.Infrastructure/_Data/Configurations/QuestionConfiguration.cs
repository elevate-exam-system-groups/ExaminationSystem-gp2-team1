using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(q => q.OrderIndex)
                .IsRequired();

            builder.Property(q => q.Explanation)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(q => q.QuizId)
                .HasDatabaseName("IX_Questions_QuizId");

            builder.HasIndex(q => new { q.QuizId, q.OrderIndex })
                .HasDatabaseName("IX_Questions_QuizId_OrderIndex");

            // Navigation properties
            builder.HasMany(q => q.Options)
                .WithOne(qo => qo.Question)
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Answers)
                .WithOne(aa => aa.Question)
                .HasForeignKey(aa => aa.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}