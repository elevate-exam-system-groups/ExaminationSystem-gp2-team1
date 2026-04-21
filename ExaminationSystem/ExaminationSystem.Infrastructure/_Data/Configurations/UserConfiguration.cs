using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired();


            builder.Property(u => u.IsEmailVerified)
                .HasDefaultValue(false);

            builder.Property(u => u.FailedLoginAttempts)
                .HasDefaultValue(0);

            builder.Property(u => u.FailedOtpAttempts)
                .HasDefaultValue(0);

            // Unique constraint on email
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email_Unique");

            // Foreign key to UserRole
            builder.HasMany(u => u.Roles)
                .WithOne(ur => ur.User)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_UserRoles");

            // Navigation properties
            builder.HasMany(u => u.OtpRecords)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.PasswordResetTokens)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.QuizAttempts)
                .WithOne(qa => qa.Student)
                .HasForeignKey(qa => qa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.DiplomaEnrollments)
                .WithOne(de => de.Student)
                .HasForeignKey(de => de.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}