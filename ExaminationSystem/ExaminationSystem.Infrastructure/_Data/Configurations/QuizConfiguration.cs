using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quizzes");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(q => q.DurationMinutes)
                .IsRequired();

            builder.Property(q => q.PassScore)
                .IsRequired()
                .HasDefaultValue(60);
     

            builder.Property(q => q.Instructions)
                .HasMaxLength(1000);

          
            // Indexes
            builder.HasIndex(q => q.DiplomaId)
                .HasDatabaseName("IX_Quizzes_DiplomaId");


            // Navigation properties
            builder.HasMany(q => q.Questions)
                .WithOne(qu => qu.Quiz)
                .HasForeignKey(qu => qu.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Attempts)
                .WithOne(qa => qa.Quiz)
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}