using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable("QuestionOptions");

            builder.HasKey(qo => qo.Id);

            builder.Property(qo => qo.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(qo => qo.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(qo => qo.OrderIndex)
                .IsRequired();

            // Indexes
            builder.HasIndex(qo => qo.QuestionId)
                .HasDatabaseName("IX_QuestionOptions_QuestionId");

            builder.HasIndex(qo => new { qo.QuestionId, qo.OrderIndex })
                .HasDatabaseName("IX_QuestionOptions_QuestionId_OrderIndex");
        }
    }
}