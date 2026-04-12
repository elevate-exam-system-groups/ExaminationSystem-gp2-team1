using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.ToTable("PasswordResetTokens");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TokenHash)
                .IsRequired();

            builder.Property(p => p.IsUsed)
                .HasDefaultValue(false);

            // Index for quick lookup
            builder.HasIndex(p => new { p.UserId, p.IsUsed })
                .HasDatabaseName("IX_PasswordResetTokens_UserId_IsUsed");

            builder.HasIndex(p => p.ExpiresAt)
                .HasDatabaseName("IX_PasswordResetTokens_ExpiresAt");
        }
    }
}